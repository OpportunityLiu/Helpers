using Opportunity.Helpers.Universal.AsyncHelpers;
using System;
using Windows.Foundation;

namespace Windows.ApplicationModel.DataTransfer
{
    /// <summary>
    /// Extension methods for <see cref="DataTransferManager"/>.
    /// </summary>
    public static class DataTransferManagerExtension
    {
        /// <summary>
        /// Show share UI.
        /// </summary>
        /// <param name="manager"><see cref="DataTransferManager"/> of sharing.</param>
        /// <param name="dataPackage"><see cref="DataPackage"/> to share.</param>
        /// <exception cref="ArgumentNullException"><paramref name="manager"/> or <paramref name="dataPackage"/> is <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException">Sharing is not supported.</exception>
        public static IAsyncAction ShareAsync(this DataTransferManager manager, DataPackage dataPackage)
        {
            if (dataPackage is null)
                throw new ArgumentNullException(nameof(dataPackage));
            return ShareAsync(manager, () => AsyncOperation<DataPackage>.CreateCompleted(dataPackage));
        }

        /// <summary>
        /// Show share UI.
        /// </summary>
        /// <param name="manager"><see cref="DataTransferManager"/> of sharing.</param>
        /// <param name="dataPackageProvider">Provides <see cref="DataPackage"/> to share.</param>
        /// <exception cref="ArgumentNullException"><paramref name="manager"/> or <paramref name="dataPackageProvider"/> is <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException">Sharing is not supported.</exception>
        public static IAsyncAction ShareAsync(this DataTransferManager manager, Func<DataPackage> dataPackageProvider)
        {
            if (dataPackageProvider is null)
                throw new ArgumentNullException(nameof(dataPackageProvider));
            return ShareAsync(manager, () => AsyncOperation<DataPackage>.CreateCompleted(dataPackageProvider()));
        }

        /// <summary>
        /// Show share UI.
        /// </summary>
        /// <param name="manager"><see cref="DataTransferManager"/> of sharing.</param>
        /// <param name="dataPackageProvider">Provides <see cref="DataPackage"/> to share.</param>
        /// <exception cref="ArgumentNullException"><paramref name="manager"/> or <paramref name="dataPackageProvider"/> is <see langword="null"/>.</exception>
        /// <exception cref="NotSupportedException">Sharing is not supported.</exception>
        public static IAsyncAction ShareAsync(this DataTransferManager manager, Func<IAsyncOperation<DataPackage>> dataPackageProvider)
        {
            if (manager is null)
                throw new ArgumentNullException(nameof(manager));
            if (dataPackageProvider is null)
                throw new ArgumentNullException(nameof(dataPackageProvider));
            if (!DataTransferManager.IsSupported())
                return AsyncAction.CreateFault(new NotSupportedException());
            var action = new AsyncAction();
            new ShareHandlerStorage(manager, dataPackageProvider, action);
            DataTransferManager.ShowShareUI();
            return action;
        }

        private class ShareHandlerStorage
        {
            public ShareHandlerStorage(DataTransferManager manager, Func<IAsyncOperation<DataPackage>> dataPackageProvider, AsyncAction action)
            {
                this.dataPackageProvider = dataPackageProvider;
                manager.DataRequested += this.T_DataRequested;
                this.action = action;
            }

            private Func<IAsyncOperation<DataPackage>> dataPackageProvider;
            private AsyncAction action;

            private async void T_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
            {
                sender.DataRequested -= this.T_DataRequested;
                if (this.action.Status == AsyncStatus.Canceled)
                    return;
                try
                {
                    var task = this.dataPackageProvider();
                    switch (task.Status)
                    {
                    case AsyncStatus.Canceled:
                        task.Close();
                        this.action.TrySetCanceled();
                        break;
                    case AsyncStatus.Completed:
                        args.Request.Data = task.GetResults();
                        task.Close();
                        this.action.TrySetResults();
                        break;
                    case AsyncStatus.Error:
                        var errorCode = task.ErrorCode;
                        this.action.TrySetException(errorCode);
                        args.Request.FailWithDisplayText(errorCode.Message);
                        task.Close();
                        break;
                    default:
                        this.action.RegisterCancellation(task.Cancel);
                        var def = args.Request.GetDeferral();
                        try
                        {
                            args.Request.Data = await task;
                            this.action.TrySetResults();
                        }
                        finally
                        {
                            def.Complete();
                        }
                        break;
                    }
                }
                catch (Exception ex)
                {
                    args.Request.FailWithDisplayText(ex.Message);
                    this.action.TrySetException(ex);
                }
            }
        }
    }
}
