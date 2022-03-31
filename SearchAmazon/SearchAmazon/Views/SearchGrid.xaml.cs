// ······································································//
// <copyright file="SearchGrid.xaml.cs" company="Jay Bautista Mendoza">  //
//     Copyright (c) Jay Bautista Mendoza. All rights reserved.          //
// </copyright>                                                          //
// ······································································//

namespace SearchAmazon.Views
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using SearchAmazon.Services;

    /// <summary>Interaction logic for SearchGrid.XAML</summary>
    public partial class SearchGrid : UserControl
    {
        #region FIELDS │ PRIVATE │ NON-STATIC │ NON-READONLY
        /// <summary>Declare an instance of resources.</summary>
        private Resources resources;

        /// <summary>Declare an instance of resourcesService.</summary>
        private ResourcesService resourcesService;
        #endregion

        #region CONSTRUCTORS │ PUBLIC
        /// <summary>Initializes a new instance of the <see cref="SearchGrid" /> class.</summary>
        public SearchGrid()
        {
            this.InitializeComponent();
            this.InitializeResources();
            this.InitializeBrands();

            this.AnyEligibility.IsChecked = true;
            this.AnyCondition.IsChecked = true;
            this.LaunchOnBrowser.IsChecked = true;
            this.ComposeUrl();

            this.PriceLow.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, this.DisablePaste));
            this.PriceHigh.CommandBindings.Add(new CommandBinding(ApplicationCommands.Paste, this.DisablePaste));
        }
        #endregion

        #region PROPERTIES │ PRIVATE │ NON-STATIC
        /// <summary>Gets the list of parameters for the URL string.</summary>
        private List<string> Parameters
        {
            get
            {
                List<string> parameters = new List<string>();

                if (!string.IsNullOrEmpty(this.FiltersParameter))
                {
                    parameters.Add(this.FiltersParameter);
                }

                return parameters;
            }
        }

        /// <summary>Gets the comma-separated string of parameter for filters.</summary>
        private string FiltersParameter
        {
            get
            {
                List<string> filters = new List<string>();

                if (!string.IsNullOrEmpty(this.Keywords.Text))
                {
                    filters.Add(string.Format(this.resources.KeywordFormat, this.Keywords.Text));
                }

                if (!string.IsNullOrEmpty(this.Brand.Text))
                {
                    filters.Add(string.Format(this.resources.BrandFormat, this.Brand.Text));
                }

                if (!string.IsNullOrEmpty(this.PriceLow.Text) || !string.IsNullOrEmpty(this.PriceHigh.Text))
                {
                    decimal low = string.IsNullOrEmpty(this.PriceLow.Text) ? 1 : decimal.Parse(this.PriceLow.Text) * 100;
                    decimal high = string.IsNullOrEmpty(this.PriceHigh.Text) ? 99999999 : decimal.Parse(this.PriceHigh.Text) * 100;
                    filters.Add(string.Format(this.resources.PriceRangeFormat, low, high));
                }

                if ((bool)this.Prime.IsChecked)
                {
                    filters.Add(this.resources.PrimeEligible);
                }

                if ((bool)this.PrimeFreeOneDay.IsChecked)
                {
                    filters.Add(this.resources.PrimeFreeOneDayEligible);
                }

                if ((bool)this.AmazonGlobal.IsChecked)
                {
                    filters.Add(this.resources.AmazonGlobalEligible);
                }

                if ((bool)this.AmazonGlobalFree.IsChecked)
                {
                    filters.Add(this.resources.AmazonGlobalEligibleFree);
                }

                if ((bool)this.AnyCondition.IsChecked)
                {
                }
                else if ((bool)this.NewCondition.IsChecked)
                {
                    filters.Add(this.resources.NewCondition);
                }
                else if ((bool)this.UsedCondition.IsChecked)
                {
                    filters.Add(this.resources.UsedCondition);
                }
                else if ((bool)this.RefurbishedCondition.IsChecked)
                {
                    filters.Add(this.resources.RefurbishedCondition);
                }

                return filters.Count <= 0
                    ? null
                    : string.Format(this.resources.EligibleFormat, HttpUtility.UrlEncode(string.Join(",", filters)));
            }
        }
        #endregion

        #region METHODS │ PRIVATE │ STATIC
        /// <summary>Checks if a text is valid size.</summary>
        /// <param name="text">Text to check.</param>
        /// <returns>Returns 'true' if valid, otherwise, returns 'false'.</returns>
        private static bool IsTextValidSize(string text)
        {
            Regex regex = new Regex("[^0-9.-]+");
            return !regex.IsMatch(text);
        }
        #endregion

        #region EVENTS │ PRIVATE │ NON-STATIC
        /// <summary>Click event for all the "Brand-" Buttons.</summary>
        /// <param name="sender">Object "sender".</param>
        /// <param name="e">RoutedEventArgs "e".</param>
        private void BrandButton_Click(object sender, RoutedEventArgs e)
        {
            string content = (sender as Button).Content.ToString().Trim().ToUpper();

            string resource = this.resources.Brands.Where(f => f.Name.Trim().ToUpper() == content.Trim().ToUpper()).Select(b => b.Value).FirstOrDefault();

            this.Brand.Text = string.IsNullOrEmpty(this.Brand.Text)
                ? resource
                : string.Join("|", new string[] { this.Brand.Text, resource });
    }

        /// <summary>Click event for all the Delete "Brand" Buttons.</summary>
        /// <param name="sender">Object "sender".</param>
        /// <param name="e">RoutedEventArgs "e".</param>
        private void DeleteBrandButton_Click(object sender, RoutedEventArgs e)
        {
            string content = (sender as Button).ToolTip.ToString().Trim().ToUpper();
            content = content.Replace("DELETE \"", string.Empty).Replace("\"", string.Empty);

            if (MessageBox.Show("Do you want to delete \"" + content + "\".", "Delete Brand", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {               
                this.resourcesService.DeleteBrand(this.resources.Brands.Where(f => f.Name.Trim().ToUpper() == content.Trim().ToUpper()).Select(b => b.Key).FirstOrDefault());
                this.resources = this.resourcesService.LoadResources();
                this.ClearBrands();
                this.InitializeBrands();
            }
        }

        /// <summary>Click event for all the Clear textbox buttons.</summary>
        /// <param name="sender">Object "sender".</param>
        /// <param name="e">RoutedEventArgs "e".</param>
        private void ClearBoxClick(object sender, RoutedEventArgs e)
        {
            string content = (sender as Button).Name.ToString().Trim().Replace("Clear", string.Empty);
            TextBox textbox = this.FindName(content) as TextBox;
            textbox.Clear();
        }

        /// <summary>TextChanged event for all changes to parameter texts.</summary>
        /// <param name="sender">Object "sender".</param>
        /// <param name="e">TextChangedEventArgs "e".</param>
        private void ParameterTextsChanged(object sender, TextChangedEventArgs e)
        {
            this.ComposeUrl();
        }

        /// <summary>Click event for all changes to parameter options.</summary>
        /// <param name="sender">Object "sender".</param>
        /// <param name="e">RoutedEventArgs "e".</param>
        private void ParameterOptionsChanged(object sender, RoutedEventArgs e)
        {
            this.ComposeUrl();
        }

        /// <summary>Disable paste functionality on a control.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">ExecutedRoutedEventArgs 'e'.</param>
        private void DisablePaste(object sender, ExecutedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        /// <summary>Preview Text Input event method for the numeric Text Boxes.</summary>
        /// <param name="sender">Object 'sender'.</param>
        /// <param name="e">TextCompositionEventArgs 'e'.</param>
        private void NumberPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !SearchGrid.IsTextValidSize(e.Text);
        }

        /// <summary>Click event for the main Search button.</summary>
        /// <param name="sender">Object "sender".</param>
        /// <param name="e">RoutedEventArgs "e".</param>
        private void UrlBox_Click(object sender, RoutedEventArgs e)
        {
            this.SubmitSearch();
        }

        /// <summary>KeyDown event of textboxes.</summary>
        /// <param name="sender">Object "sender".</param>
        /// <param name="e">KeyEventArgs "e".</param>
        private void SearchKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    this.SubmitSearch();
                    break;
                default: break;
            }
        }

        /// <summary>Click event of FavoriteBrand button.</summary>
        /// <param name="sender">Object "sender".</param>
        /// <param name="e">RoutedEventArgs "e".</param>
        private void FavoriteBrand_Click(object sender, RoutedEventArgs e)
        {
            string text = this.Brand.Text;

            if (!string.IsNullOrEmpty(text) || !text.Contains('|'))
            {
                if (this.resources.Brands == null || !this.resources.Brands.Any(b => b.Value.ToUpper() == text.ToUpper().Trim()))
                {
                    this.resourcesService.AddBrand(this.Brand.Text);
                    this.resources = this.resourcesService.LoadResources();

                    this.ClearBrands();
                    this.InitializeBrands();
                }                
            }
        }
        #endregion

        #region METHODS │ PRIVATE │ NON-STATIC
        /// <summary>Load all resources from the XML file.</summary>
        private void InitializeResources()
        {
            this.resourcesService = new ResourcesService();
            this.resources = this.resourcesService.LoadResources();
        }

        /// <summary>Create dynamic buttons for brands.</summary>
        private void InitializeBrands()
        {
            int brandsCount = this.resources.Brands.Count;

            if (brandsCount == 0)
            {
                this.BrandFavorites.Visibility = System.Windows.Visibility.Collapsed;
            }

            for (int index = 0; index < brandsCount; index++)
            {
                TextBlock textBlock = new TextBlock();
                textBlock.Text = "  ↑";
                textBlock.Style = this.FindResource("LabelStyle") as Style;
                
                Button button = new Button();
                button.Content = this.resources.Brands[index].Name;
                button.Style = this.FindResource("LinkButtonStyle") as Style;    
                button.ToolTip = string.Concat("Click to include ", button.Content, " in brand-enabled search.");
                button.Click += this.BrandButton_Click;

                Button deleteBrandButton = new Button();
                deleteBrandButton.Content = "×";
                deleteBrandButton.Style = this.FindResource("ButtonStyle") as Style;
                deleteBrandButton.Height = 10;
                deleteBrandButton.Width = 10;
                deleteBrandButton.ToolTip = "Delete \"" + this.resources.Brands[index].Name + "\"";
                deleteBrandButton.Click += this.DeleteBrandButton_Click;

                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;
                stackPanel.Children.Add(textBlock);
                stackPanel.Children.Add(button);
                stackPanel.Children.Add(deleteBrandButton);
                
                this.BrandFavorites.Children.Insert(index, stackPanel);
            }
        }

        /// <summary>Clear all brands from the UI.</summary>
        private void ClearBrands()
        {
            this.BrandFavorites.Children.Clear();
        }

        /// <summary>Compose the URL based on parameters and rename the button for copying.</summary>
        private void ComposeUrl()
        {
            if (this.Parameters.Count <= 0)
            {
                this.UrlBox.Content = this.resources.AmazonUrl;
                return;
            }

            this.UrlBox.Content = string.Format(this.resources.AmazonUrlP, string.Join("&", this.Parameters));
        }

        /// <summary>Submits the search.</summary>
        private void SubmitSearch()
        {
            if ((bool)this.CopyToClipboard.IsChecked)
            {
                Clipboard.SetData(DataFormats.Text, this.UrlBox.Content);
            }

            if ((bool)this.LaunchOnBrowser.IsChecked)
            {
                string browser = ConfigurationManager.AppSettings.Get("browser");

                if (browser.ToUpper() == "DEFAULT")
                {
                    Process.Start(this.UrlBox.Content.ToString());
                }
                else
                {
                    using (Process process = new Process())
                    {
                        process.StartInfo.FileName = ConfigurationManager.AppSettings.Get("browser");
                        process.StartInfo.Arguments = string.Format(ConfigurationManager.AppSettings.Get("urlformat"), this.UrlBox.Content.ToString());

                        process.Start();
                    }
                }
            }
        }
        #endregion        
    }
}