﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Resx = GitWrite.Properties.Resources;

namespace GitWrite.Views.Controls
{
   public partial class StyledDialog : UserControl
   {
      private DialogResult _dialogResult;
      private Window _modalWindow;
      private bool _hasPlayedExitAnimation;

      public StyledDialog()
      {
         InitializeComponent();
      }

      public DialogResult ShowDialog( string title, string message, DialogButtons buttons )
      {
         TitleTextBlock.Text = title;
         MessageTextBlock.Text = message;

         SetupButtons( buttons );

         var mainWindow = Application.Current.MainWindow;

         _modalWindow = new Window
         {
            AllowsTransparency = true,
            Background = Brushes.Transparent,
            Content = this,
            Height = 230,
            Owner = mainWindow,
            ShowInTaskbar = false,
            Width = 400,
            WindowStartupLocation = WindowStartupLocation.CenterScreen,
            WindowStyle = WindowStyle.None
         };

         _modalWindow.KeyDown += ModalWindowKeyDown;
         _modalWindow.Closing += ModalWindowClosing;
         _modalWindow.ShowDialog();

         return _dialogResult;
      }

      private void ModalWindowClosing( object sender, CancelEventArgs e )
      {
         if ( _hasPlayedExitAnimation )
         {
            return;
         }

         e.Cancel = true;
         _hasPlayedExitAnimation = true;

         var exitStoryboard = (Storyboard) Resources["ExitStoryboard"];
         exitStoryboard.Completed += ( _, __ ) => _modalWindow.Close();
         exitStoryboard.Begin();
      }

      private void SetupButtons( DialogButtons buttons )
      {
         switch ( buttons )
         {
            case DialogButtons.OK:
            {
               CreateButton( Resx.OKText, DialogResult.OK );
               break;
            }
            case DialogButtons.YesNo:
            {
               CreateButton( Resx.YesText, DialogResult.Yes );
               CreateButton( Resx.NoText, DialogResult.No );
               break;
            }
            case DialogButtons.YesNoCancel:
            {
               CreateButton( Resx.YesText, DialogResult.Yes );
               CreateButton( Resx.NoText, DialogResult.No );
               CreateButton( Resx.CancelText, DialogResult.Cancel );
               break;
            }
            case DialogButtons.SaveDiscardCancel:
            {
               CreateButton( Resx.SaveText, DialogResult.Save );
               CreateButton( Resx.DiscardText, DialogResult.Discard );
               CreateButton( Resx.CancelText, DialogResult.Cancel );
               break;
            }
         }
      }

      private void CreateButton( string text, DialogResult dialogResult )
      {
         var button = new Button
         {
            Height = 30,
            HorizontalAlignment = HorizontalAlignment.Right,
            Margin = new Thickness( 0, 0, 6, 0 ),
            Tag = dialogResult,
            VerticalAlignment = VerticalAlignment.Bottom,
            Width = 80,
         };

         button.Click += Button_OnClick;

         var grid = new Grid
         {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch
         };

         var accessText = new AccessText
         {
            Foreground = Brushes.White,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Text = text
         };

         grid.Children.Add( accessText );

         button.Content = grid;

         ButtonPanel.Children.Add( button );
      }

      private void Button_OnClick( object sender, EventArgs e )
      {
         var button = (Button) sender;
         AnimateButton( button );

         _dialogResult = (DialogResult) button.Tag;
      }

      private void AnimateButton( Button button )
      {
         var ellipse = new Ellipse
         {
            Width = 0,
            Height = 0,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Fill = new SolidColorBrush( Color.FromArgb( 64, 255, 255, 255 ) )
         };

         var grid = (Grid) button.Content;
         grid.Children.Add( ellipse );

         var storyboard = new Storyboard();

         var widthAnimation = new DoubleAnimation( 0, button.ActualWidth * 2, new Duration( TimeSpan.FromMilliseconds( 250 ) ) );
         var heightAnimation = new DoubleAnimation( 0, button.ActualWidth * 2, widthAnimation.Duration );
         var opacityAnimation = new DoubleAnimation( 1, 0, new Duration( TimeSpan.FromMilliseconds( 200 ) ) )
         {
            BeginTime = TimeSpan.FromMilliseconds( 80 )
         };

         Storyboard.SetTarget( widthAnimation, ellipse );
         Storyboard.SetTargetProperty( widthAnimation, new PropertyPath( WidthProperty ) );

         Storyboard.SetTarget( heightAnimation, ellipse );
         Storyboard.SetTargetProperty( heightAnimation, new PropertyPath( HeightProperty ) );

         Storyboard.SetTarget( opacityAnimation, ellipse );
         Storyboard.SetTargetProperty( opacityAnimation, new PropertyPath( OpacityProperty ) );

         storyboard.Children.Add( widthAnimation );
         storyboard.Children.Add( heightAnimation );
         storyboard.Children.Add( opacityAnimation );

         storyboard.Completed += ( _, __ ) => _modalWindow.Close();
         storyboard.Begin();
      }

      private void ModalWindowKeyDown( object sender, KeyEventArgs e )
      {
         if ( e.Key == Key.Escape )
         {
            _dialogResult = DialogResult.Cancel;
            _modalWindow.Close();
         }
      }
   }
}
