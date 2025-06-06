﻿using Syncfusion.Maui.Toolkit.Graphics.Internals;
using Syncfusion.Maui.Toolkit.Themes;

namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Represents a class which is used to customize all the properties of column header view of the SfPicker.
    /// </summary>
    public class PickerColumnHeaderView : Element, IThemeElement
    {
        #region Bindable Properties

        /// <summary>
        /// Identifies the <see cref="Height"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Height"/> dependency property.
        /// </value>
        public static readonly BindableProperty HeightProperty =
            BindableProperty.Create(
                nameof(Height),
                typeof(double),
                typeof(PickerColumnHeaderView),
                0d,
                propertyChanged: OnHeightChanged);

        /// <summary>
        /// Identifies the <see cref="TextStyle"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="TextStyle"/> dependency property.
        /// </value>
        public static readonly BindableProperty TextStyleProperty =
            BindableProperty.Create(
                nameof(TextStyle),
                typeof(PickerTextStyle),
                typeof(PickerColumnHeaderView),
                defaultValueCreator: bindable => GetColumnHeaderTextStyle(bindable),
                propertyChanged: OnTextStyleChanged);

        /// <summary>
        /// Identifies the <see cref="Background"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="Background"/> dependency property.
        /// </value>
        public static readonly BindableProperty BackgroundProperty =
            BindableProperty.Create(
                nameof(Background),
                typeof(Brush),
                typeof(PickerColumnHeaderView),
                defaultValueCreator: bindable => new SolidColorBrush(Color.FromArgb("#F7F2FB")),
                propertyChanged: OnBackgroundChanged);

        /// <summary>
        /// Identifies the <see cref="DividerColor"/> dependency property.
        /// </summary>
        /// <value>
        /// The identifier for <see cref="DividerColor"/> dependency property.
        /// </value>
        public static readonly BindableProperty DividerColorProperty =
            BindableProperty.Create(
                nameof(DividerColor),
                typeof(Color),
                typeof(PickerColumnHeaderView),
                defaultValueCreator: bindable => Color.FromArgb("#CAC4D0"),
                propertyChanged: OnDividerColorChanged);

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PickerColumnHeaderView"/> class.
        /// </summary>
        public PickerColumnHeaderView()
        {
            ThemeElement.InitializeThemeResources(this, "SfPickerTheme");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value to specify the height of column header view on SfPicker.
        /// </summary>
        /// <value>The default value of <see cref="PickerColumnHeaderView.Height"/> is 0d.</value>
        /// <example>
        /// The following example demonstrates how to set the height of the column header view.
        /// # [XAML](#tab/tabid-1)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfPicker>
        ///     <picker:SfPicker.ColumnHeaderView>
        ///         <picker:PickerColumnHeaderView Height="50" />
        ///     </picker:SfPicker.ColumnHeaderView>
        /// </picker:SfPicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-2)
        /// <code language="C#">
        /// SfPicker picker = new SfPicker();
        /// picker.ColumnHeaderView = new PickerColumnHeaderView
        /// {
        ///     Height = 50
        /// };
        /// </code>
        /// </example>
        public double Height
        {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text style of the column header text in SfPicker.
        /// </summary>
        /// <example>
        /// The following example demonstrates how to set the text style of the column header view.
        /// # [XAML](#tab/tabid-3)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfPicker>
        ///     <picker:SfPicker.ColumnHeaderView>
        ///         <picker:PickerColumnHeaderView>
        ///             <picker:PickerColumnHeaderView.TextStyle>
        ///                 <picker:PickerTextStyle TextColor="Blue" FontSize="16" />
        ///             </picker:PickerColumnHeaderView.TextStyle>
        ///         </picker:PickerColumnHeaderView>
        ///     </picker:SfPicker.ColumnHeaderView>
        /// </picker:SfPicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-4)
        /// <code language="C#">
        /// SfPicker picker = new SfPicker();
        /// picker.ColumnHeaderView = new PickerColumnHeaderView
        /// {
        ///     TextStyle = new PickerTextStyle
        ///     {
        ///         TextColor = Colors.Blue,
        ///         FontSize = 16
        ///     }
        /// };
        /// </code>
        /// </example>
        public PickerTextStyle TextStyle
        {
            get { return (PickerTextStyle)GetValue(TextStyleProperty); }
            set { SetValue(TextStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background of the column header view in SfPicker.
        /// </summary>
        /// <value>The default value <see cref="PickerColumnHeaderView.Background"/> is "#F7F2FB".</value>
        /// <example>
        /// The following example demonstrates how to set the background of the column header view.
        /// # [XAML](#tab/tabid-5)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfPicker>
        ///     <picker:SfPicker.ColumnHeaderView>
        ///         <picker:PickerColumnHeaderView Background="#E0E0E0" />
        ///     </picker:SfPicker.ColumnHeaderView>
        /// </picker:SfPicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-6)
        /// <code language="C#">
        /// SfPicker picker = new SfPicker();
        /// picker.ColumnHeaderView = new PickerColumnHeaderView
        /// {
        ///     Background = new SolidColorBrush(Color.FromHex("#E0E0E0"))
        /// };
        /// </code>
        /// </example>
        public Brush Background
        {
            get { return (Brush)GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the background of the column header separator line background in SfPicker.
        /// </summary>
        /// <value>The default value of <see cref="PickerColumnHeaderView.DividerColor"/> is "#CAC4D0".</value>
        /// <example>
        /// The following example demonstrates how to set the divider color of the column header view.
        /// # [XAML](#tab/tabid-7)
        /// <code language="xaml">
        /// <![CDATA[
        /// <picker:SfPicker>
        ///     <picker:SfPicker.ColumnHeaderView>
        ///         <picker:PickerColumnHeaderView DividerColor="Gray" />
        ///     </picker:SfPicker.ColumnHeaderView>
        /// </picker:SfPicker>
        /// ]]>
        /// </code>
        /// # [C#](#tab/tabid-8)
        /// <code language="C#">
        /// SfPicker picker = new SfPicker();
        /// picker.ColumnHeaderView = new PickerColumnHeaderView
        /// {
        ///     DividerColor = Colors.Gray
        /// };
        /// </code>
        /// </example>
        public Color DividerColor
        {
            get { return (Color)GetValue(DividerColorProperty); }
            set { SetValue(DividerColorProperty, value); }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Need to update the parent for the new value.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        void SetParent(Element? oldValue, Element? newValue)
        {
            if (oldValue != null)
            {
                oldValue.Parent = null;
            }

            if (newValue != null)
            {
                newValue.Parent = this;
            }
        }

        #endregion

        #region Property Changed Methods

        /// <summary>
        /// Method invokes on the picker column header height changed.
        /// </summary>
        /// <param name="bindable">The column header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnHeightChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PickerColumnHeaderView)?.RaisePropertyChanged(nameof(Height));
        }

        /// <summary>
        /// Method invokes on picker column header text style property changed.
        /// </summary>
        /// <param name="bindable">The column header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is PickerColumnHeaderView columnHeaderView)
            {
                columnHeaderView.SetParent(oldValue as Element, newValue as Element);
            }

            (bindable as PickerColumnHeaderView)?.RaisePropertyChanged(nameof(TextStyle), oldValue);
        }

        /// <summary>
        /// Method invokes on the picker column header background changed.
        /// </summary>
        /// <param name="bindable">The column header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnBackgroundChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PickerColumnHeaderView)?.RaisePropertyChanged(nameof(Background));
        }

        /// <summary>
        /// Method invokes on the picker column header separator line background changed.
        /// </summary>
        /// <param name="bindable">The column header settings object.</param>
        /// <param name="oldValue">Property old value.</param>
        /// <param name="newValue">Property new value.</param>
        static void OnDividerColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as PickerColumnHeaderView)?.RaisePropertyChanged(nameof(DividerColor));
        }

        /// <summary>
        /// Method to get the default text style for the column header view.
        /// </summary>
        /// <returns>Returns the default column header text style.</returns>
        static ITextElement GetColumnHeaderTextStyle(BindableObject bindable)
        {
            var columnHeader = (PickerColumnHeaderView)bindable;
            var pickerTextStyle = new PickerTextStyle()
            {
                FontSize = 14,
                TextColor = Color.FromArgb("#49454F"),
                Parent = columnHeader,
            };

            return pickerTextStyle;
        }

        /// <summary>
        /// Method to invoke picker property changed event on column header settings properties changed.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <param name="oldValue">Property old value.</param>
        void RaisePropertyChanged(string propertyName, object? oldValue = null)
        {
            PickerPropertyChanged?.Invoke(this, new PickerPropertyChangedEventArgs(propertyName) { OldValue = oldValue });
        }

        #endregion

        #region Interface Implementation

        /// <summary>
        /// This method will be called when a theme dictionary
        /// that contains the value for your control key is merged in application.
        /// </summary>
        /// <param name="oldTheme">The old value.</param>
        /// <param name="newTheme">The new value.</param>
        void IThemeElement.OnCommonThemeChanged(string oldTheme, string newTheme)
        {
        }

        /// <summary>
        /// This method will be called when users merge a theme dictionary
        /// that contains value for “SyncfusionTheme” dynamic resource key.
        /// </summary>
        /// <param name="oldTheme">Old theme.</param>
        /// <param name="newTheme">New theme.</param>
        void IThemeElement.OnControlThemeChanged(string oldTheme, string newTheme)
        {
        }

        #endregion

        #region Events

        /// <summary>
        /// Event Invokes on column header settings property changed and this includes old value of the changed property which is used to unwire events for nested classes.
        /// </summary>
        internal event EventHandler<PickerPropertyChangedEventArgs>? PickerPropertyChanged;

        #endregion
    }
}