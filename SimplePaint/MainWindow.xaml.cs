using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            BrushColorCombo.ItemsSource = typeof(Colors).GetProperties();
            PropertyInfo[] colors = BrushColorCombo.ItemsSource.Cast<PropertyInfo>().ToArray();
            for (int i = 0; i < colors.Length; i++)
            {
                if (colors[i].Name == "Black")
                {
                    BrushColorCombo.SelectedIndex = i;
                    break;
                }
            }
        }

        private void BrushSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (PaintCanvas == null) return;

            var drawingAttributes = PaintCanvas.DefaultDrawingAttributes;
            drawingAttributes.Width = BrushSlider.Value;
            drawingAttributes.Height = BrushSlider.Value;
            PaintCanvas.EraserShape = new RectangleStylusShape(BrushSlider.Value, BrushSlider.Value);

            // From the help:
            // "If you change the EraserShape, the cursor rendered on the InkCanvas is not updated until the next EditingMode change."
            var previousEditingMode = PaintCanvas.EditingMode;
            PaintCanvas.EditingMode = InkCanvasEditingMode.None;
            PaintCanvas.EditingMode = previousEditingMode;
        }

        private void BrushColorCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Color selectedColor = (Color)(BrushColorCombo.SelectedItem as PropertyInfo).GetValue(null, null);
            PaintCanvas.DefaultDrawingAttributes.Color = selectedColor;
        }

        private void BrushStateCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PaintCanvas == null) return;

            // HACK: This is very hacky. Doing this as I know the order of the ComboBoxItems on the UI.
            // Best way would probably to get the selected item as an enum value.
            switch (BrushStateCombo.SelectedIndex)
            {
                case 0:
                    PaintCanvas.EditingMode = InkCanvasEditingMode.Ink;
                    break;
                case 1:
                    PaintCanvas.EditingMode = InkCanvasEditingMode.Select;
                    break;
                case 2:
                    PaintCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
                    break;
                case 3:
                    PaintCanvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
                    break;
            }
        }

        private void BrushShapesCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PaintCanvas == null) return;

            // HACK: This is very hacky. Doing this as I know the order of the ComboBoxItems on the UI.
            // Best way would probably to get the selected item as an enum value.
            switch (BrushShapesCombo.SelectedIndex)
            {
                case 0:
                    PaintCanvas.DefaultDrawingAttributes.StylusTip = StylusTip.Ellipse;
                    break;
                case 1:
                    PaintCanvas.DefaultDrawingAttributes.StylusTip = StylusTip.Rectangle;
                    break;
            }
        }
    }
}
