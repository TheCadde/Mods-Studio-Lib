using System.Windows;
using System.Windows.Controls;

using ModsStudioLib.Databases;
using ModsStudioLib.Definitions.Parsing;

namespace DefinitionDatabaseEditor {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {

        public MainWindow() {
            InitializeComponent();

            DefinitionDatabase.ReadDatabase();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e) {
            Structures.ItemsSource = DefinitionDatabase.StructureDescriptors.Values;
        }

        private void Structures_OnBeginningEdit(object sender, DataGridBeginningEditEventArgs e) {
            e.Cancel = true;
        }

        private void Structures_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (e.AddedItems.Count == 0)
                return;

            var item = (DefinitionStructureDescriptor)e.AddedItems[0];

            Values.ItemsSource = item.ValueDescriptors.Values;
            StructureSettings.DataContext = item;
            VariableSettings.Visibility = Visibility.Hidden;
            StructureSettings.Visibility = Visibility.Visible;
        }

        private void Values_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (e.AddedItems.Count == 0)
                return;

            var item = (DefinitionValueDescriptor)e.AddedItems[0];

            VariableSettings.DataContext = item;
            StructureSettings.Visibility = Visibility.Hidden;
            VariableSettings.Visibility = Visibility.Visible;
        }
    }
}
