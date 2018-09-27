using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace System
{
    /// <summary>
    /// Factory class to create weak delegates.
    /// </summary>
    public static class WeakDelegate
    {
        /// <summary>
        /// Create a weak delegate from a delegate.
        /// </summary>
        /// <typeparam name="T">Type of delegate.</typeparam>
        /// <param name="dele">Delegate to create weak delegate from.</param>
        /// <returns>A weak delegate that wraps target of <paramref name="dele"/> with <see cref="WeakReference"/>.</returns>
        public static T Create<T>(T dele)
            where T : Delegate
            => Create(dele, false);

        /// <summary>
        /// Create a weak delegate from a delegate.
        /// </summary>
        /// <typeparam name="T">Type of delegate.</typeparam>
        /// <param name="dele">Delegate to create weak delegate from.</param>
        /// <param name="throwIfDestoryed">Throw if the <see cref="WeakReference"/> is not alive.</param>
        /// <returns>A weak delegate that wraps target of <paramref name="dele"/> with <see cref="WeakReference"/>.</returns>
        public static T Create<T>(T dele, bool throwIfDestoryed)
            where T : Delegate
        {
            if (dele is null)
                throw new ArgumentNullException(nameof(dele));
            var builder = new WeakDelegateBuilder<T>(false);
            var childs = dele.GetInvocationList();
            for (var i = 0; i < childs.Length; i++)
            {
                childs[i] = builder.CreateOne((T)childs[i]);
            }
            return (T)Delegate.Combine(childs);
        }

        private ref struct WeakDelegateBuilder<T>
            where T : Delegate
        {
            public WeakDelegateBuilder(bool throwIfDestoryed) : this()
            {
                this.throwIfDestoryed = throwIfDestoryed;
            }

            private Type DelegateReturnType;
            private Expression WeakDeadExpression;
            private ParameterExpression[] DelegateParams;

            private readonly bool throwIfDestoryed;

            private void initDeleInfo()
            {
                if (this.DelegateReturnType is null)
                {
                    var dele = typeof(T).GetTypeInfo().GetDeclaredMethod("Invoke");
                    this.DelegateParams = dele.GetParameters().Select(p => Expression.Parameter(p.ParameterType, p.Name)).ToArray();
                    this.DelegateReturnType = dele.ReturnType;
                    this.WeakDeadExpression = this.throwIfDestoryed
                        ? throwObjectDisconnected
                        : Expression.Default(this.DelegateReturnType);
                }
            }

            public T CreateOne(T expression)
            {
                var target = expression.Target;
                if (target is null || target is ValueType)
                    return expression;

                initDeleInfo();

                var method = expression.GetMethodInfo();

                var weakTarget = Expression.Constant(new WeakReference(target));
                var isAlive = Expression.Property(weakTarget, weakRefIsAlive);
                var callTarget = Expression.Convert(Expression.Property(weakTarget, weakRefTarget), method.DeclaringType);

                var body = Expression.Condition(isAlive,
                    Expression.Call(callTarget, method, this.DelegateParams),
                    this.WeakDeadExpression,
                    this.DelegateReturnType);
                return Expression.Lambda<T>(body, this.DelegateParams).Compile();
            }

            private static readonly PropertyInfo weakRefIsAlive = typeof(WeakReference).GetRuntimeProperty("IsAlive");
            private static readonly PropertyInfo weakRefTarget = typeof(WeakReference).GetRuntimeProperty("Target");

            private static readonly Expression throwObjectDisconnected = createThrowObjectDisconnected();
            private static Expression createThrowObjectDisconnected()
            {
                var conExConsturctor = typeof(COMException).GetTypeInfo().DeclaredConstructors.Single(c =>
                {
                    var p = c.GetParameters();
                    if (p.Length != 2)
                        return false;
                    return p[0].ParameterType == typeof(string) && p[1].ParameterType == typeof(int);
                });
                const int RPC_E_DISCONNECTED = unchecked((int)0x80010108);
                return Expression.Throw(Expression.New(conExConsturctor, Expression.Constant("The target of WeakDelegate has destroyed."), Expression.Constant(RPC_E_DISCONNECTED)));
            }
        }
    }
}
