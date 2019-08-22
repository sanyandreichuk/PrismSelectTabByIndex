using System;
using System.Threading.Tasks;
using Prism.Common;
using Prism.Navigation;
using Xamarin.Forms;


namespace PrismSelectTabByIndex.Extensions
{
    public static class INavigationServiceExtensions
    {
        /// <summary>
        /// Selects a Tab of the TabbedPage parent.
        /// </summary>
        /// <param name="name">The name of the tab to select</param>
        /// <param name="parameters">The navigation parameters</param>
        public static Task<INavigationResult> SelectTabAsync(this INavigationService navigationService, string name, INavigationParameters parameters = null)
        {
            return SelectTabAsync(navigationService, parameters, (tabbedPage) => GetTargetPage(tabbedPage, name));
        }

        /// <summary>
        /// Selects a Tab of the TabbedPage parent.
        /// </summary>
        /// <param name="index">The index of the tab to select</param>
        /// <param name="parameters">The navigation parameters</param>
        public static Task<INavigationResult> SelectTabAsync(this INavigationService navigationService, int index, INavigationParameters parameters = null)
        {
            return SelectTabAsync(navigationService, parameters, (tabbedPage) => GetTargetPage(tabbedPage, index));
        }       

        private static async Task<INavigationResult> SelectTabAsync(INavigationService navigationService, INavigationParameters parameters, Func<TabbedPage, Page> targetPageProvider)
        {
            try {
                var currentPage = ((IPageAware)navigationService).Page;

                var canNavigate = await PageUtilities.CanNavigateAsync(currentPage, parameters);
                if (!canNavigate)
                    throw new Exception($"IConfirmNavigation for {currentPage} returned false");

                TabbedPage tabbedPage = null;

                if (currentPage.Parent is TabbedPage parent) {
                    tabbedPage = parent;
                } else if (currentPage.Parent is NavigationPage navPage) {
                    if (navPage.Parent != null && navPage.Parent is TabbedPage parent2) {
                        tabbedPage = parent2;
                    }
                }

                if (tabbedPage == null)
                    throw new Exception("No parent TabbedPage could be found");

                Page target = targetPageProvider.Invoke(tabbedPage);

                var tabParameters = UriParsingHelper.GetSegmentParameters(target.GetType().Name, parameters);

                tabbedPage.CurrentPage = target;
                PageUtilities.OnNavigatedFrom(currentPage, tabParameters);
                PageUtilities.OnNavigatedTo(target, tabParameters);

            } catch (Exception ex) {
                return new NavigationResult { Exception = ex };
            }

            return new NavigationResult { Success = true };
        }

        private static Page GetTargetPage(TabbedPage tabbedPage, int index)
        {
            Page target = null;
            if (index >= 0 && index < tabbedPage.Children.Count)
                target = tabbedPage.Children[index];

            if (target is null)
                throw new Exception($"Could not find a Child Tab for index '{index}'");

            return target;
        }

        private static Page GetTargetPage(TabbedPage tabbedPage, string name)
        {
            var tabToSelectedType = PageNavigationRegistry.GetPageType(UriParsingHelper.GetSegmentName(name));
            if (tabToSelectedType is null)
                throw new Exception($"No View Type has been registered for '{name}'");

            Page target = null;
            foreach (var child in tabbedPage.Children) {
                if (child.GetType() == tabToSelectedType) {
                    target = child;
                    break;
                }

                if (child is NavigationPage childNavPage) {
                    if (childNavPage.CurrentPage.GetType() == tabToSelectedType ||
                        childNavPage.RootPage.GetType() == tabToSelectedType) {
                        target = child;
                        break;
                    }
                }
            }

            if (target is null)
                throw new Exception($"Could not find a Child Tab for '{name}'");

            return target;
        }
    }
}
