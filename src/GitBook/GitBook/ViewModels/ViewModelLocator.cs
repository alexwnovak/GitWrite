/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:GitBook"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using Microsoft.Practices.ServiceLocation;

namespace GitBook.ViewModels
{
   /// <summary>
   /// This class contains static references to all the view models in the
   /// application and provides an entry point for the bindings.
   /// </summary>
   public class ViewModelLocator
   {
      public MainViewModel MainViewModel
      {
         get
         {
            return ServiceLocator.Current.GetInstance<MainViewModel>();
         }
      }

      public static void Cleanup()
      {
         // TODO Clear the ViewModels
      }
   }
}