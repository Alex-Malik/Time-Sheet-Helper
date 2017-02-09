using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TimeSheet.Views.Controls
{
    public class Message : Control
    {
        #region Static Members
        static Message()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Message),
                new FrameworkPropertyMetadata(typeof(Message)));
        }

        public static readonly DependencyProperty TypeNameProperty =
            DependencyProperty.Register(nameof(TypeName), typeof(String), typeof(Message), new PropertyMetadata(null));

        public static readonly DependencyProperty TypeGeometryProperty =
            DependencyProperty.Register(nameof(TypeGeometry), typeof(Geometry), typeof(Message), new PropertyMetadata(null));

        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.Register(nameof(CloseCommand), typeof(ICommand), typeof(Message), new PropertyMetadata(null));

        public static readonly DependencyProperty FontBrushProperty =
            DependencyProperty.Register(nameof(FontBrush), typeof(Brush), typeof(Message), new PropertyMetadata(null));

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(nameof(Content), typeof(Object), typeof(Message), new PropertyMetadata(null));
        #endregion

        #region Properties
        public String TypeName
        {
            get { return (String)GetValue(TypeNameProperty); }
            set { SetValue(TypeNameProperty, value); }
        }

        public Geometry TypeGeometry
        {
            get { return (Geometry)GetValue(TypeGeometryProperty); }
            set { SetValue(TypeGeometryProperty, value); }
        }
        
        public ICommand CloseCommand
        {
            get { return (ICommand)GetValue(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }
        
        public Brush FontBrush
        {
            get { return (Brush)GetValue(FontBrushProperty); }
            set { SetValue(FontBrushProperty, value); }
        }
        
        public Object Content
        {
            get { return (Object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        #endregion

        #region Public Methods

        #endregion
    }
}
