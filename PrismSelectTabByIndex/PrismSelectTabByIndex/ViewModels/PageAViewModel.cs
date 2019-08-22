using Prism.Commands;
using Prism.Navigation;
using PrismSelectTabByIndex.Extensions;


namespace PrismSelectTabByIndex.ViewModels
{
    public class PageAViewModel : ViewModelBase
    {
        public DelegateCommand SelectATabCommand { get; set; }
        public DelegateCommand SelectBTabCommand { get; set; }
        public DelegateCommand Select0TabCommand { get; set; }
        public DelegateCommand Select1TabCommand { get; set; }
        public DelegateCommand Select2TabCommand { get; set; }

        public PageAViewModel(INavigationService navigationService) : base(navigationService)
        {
            SelectATabCommand = new DelegateCommand(SelectATabExecute);
            SelectBTabCommand = new DelegateCommand(SelectBTabExecute);
            Select0TabCommand = new DelegateCommand(Select0TabExecute);
            Select1TabCommand = new DelegateCommand(Select1TabExecute);
            Select2TabCommand = new DelegateCommand(Select2TabExecute);
        }

        private void SelectATabExecute()
        {
            NavigationService.SelectTabAsync("PageA");
        }

        private void SelectBTabExecute()
        {
            NavigationService.SelectTabAsync("PageB");
        }

        private void Select0TabExecute()
        {
            NavigationService.SelectTabAsync(0);
        }

        private void Select1TabExecute()
        {
            NavigationService.SelectTabAsync(1);
        }

        private void Select2TabExecute()
        {
            NavigationService.SelectTabAsync(2);
        }
    }
}
