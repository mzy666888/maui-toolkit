﻿using System.Collections;
using System.Collections.ObjectModel;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Picker
{
    /// <summary>
    /// Represents a class which contains picker view helper.
    /// </summary>
    internal static class PickerHelper
    {
        #region Internal Methods

        /// <summary>
        /// Method to convert brush to color.
        /// </summary>
        /// <param name="brush">The brush to convert.</param>
        /// <returns>Returns the color value.</returns>
        internal static Color ToColor(this Brush brush)
        {
            Paint paint = (Paint)brush;
            return paint.ToColor() ?? Colors.Transparent;
        }

        /// <summary>
        /// Method to trim the text based on available width.
        /// </summary>
        /// <param name="text">The text to trim.</param>
        /// <param name="width">The available width.</param>
        /// <param name="textStyle">The text style.</param>
        /// <returns>Returns the text for the available width.</returns>
        internal static string TrimText(string text, double width, PickerTextStyle textStyle)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            double value = 0;
            var textTrim = text;
            Size textSize = text.Measure(textStyle);
            var ellipsisWidth = "..".Measure(textStyle).Width;

            do
            {
                if (textSize.Width > width && text.Length > 0)
                {
                    int length = text.Length - 2;
                    text = text.Substring(0, length < 0 ? 0 : length);
                    value = text.Measure(textStyle).Width;
                }
                else if (width - ellipsisWidth < 0)
                {
                    break;
                }
                else
                {
                    continue;
                }
            }
            while (value > width - ellipsisWidth && text.Length > 0);

            if (textTrim != text)
            {
                textTrim = text + "..";
            }

            return textTrim;
        }

        /// <summary>
        /// Method to return the items count from items source object.
        /// </summary>
        /// <param name="itemssSource">The items source object.</param>
        /// <returns>Returns items count from items source object.</returns>
        internal static int GetItemsCount(object itemssSource)
        {
            if (itemssSource != null && itemssSource is ICollection collection)
            {
                return collection.Count;
            }

            return 0;
        }

        /// <summary>
        /// Method to return the items value count from items source object.
        /// </summary>
        /// <param name="column">The picker column value.</param>
        /// <returns>Returns items value count from items source object.</returns>
        internal static int GetSelectedItemIndex(PickerColumn column)
        {
            if (column.ItemsSource != null && column.ItemsSource is ICollection collection && column.SelectedItem != null)
            {
                int index = 0;
                foreach (object value in collection)
                {
                    //// At initial, not need to check the selected index value for selected item.
                    if (column._isSelectedItemChanged && column.SelectedItem.Equals(value))
                    {
                        return index;
                    }
                    //// In dynamic, need to check the selected item index based on selected index.
                    else if (!column._isSelectedItemChanged && column.SelectedItem.Equals(value) && index == column.SelectedIndex)
                    {
                        return index;
                    }

                    index++;
                }
            }

            return column.SelectedItem == null ? -1 : 0;
        }

        /// <summary>
        /// Method invokes to get the default value for selected item based on selected index.
        /// </summary>
        /// <param name="bindable">The bindable value.</param>
        /// <returns>Returns the index value.</returns>
        internal static object? GetSelectedItemDefaultValue(BindableObject bindable)
        {
            if (bindable is PickerColumn pickerColumn && pickerColumn != null)
            {
                if (pickerColumn.ItemsSource != null && pickerColumn.ItemsSource is ICollection collection && pickerColumn.SelectedIndex > -1)
                {
                    int index = 0;
                    object firstItem = string.Empty;
                    foreach (object item in collection)
                    {
                        if (index == 0)
                        {
                            firstItem = item;
                        }

                        if (index == pickerColumn.SelectedIndex)
                        {
                            return item;
                        }

                        index++;
                    }

                    return firstItem;
                }
            }

            return null;
        }

        /// <summary>
        /// Method to get the valid selected index.
        /// </summary>
        /// <param name="selectedIndex">The selected index.</param>
        /// <param name="itemsCount">The items count.</param>
        /// <returns>Returns the valid selected index.</returns>
        internal static int GetValidSelectedIndex(int selectedIndex, int itemsCount)
        {
            //// Considered the selected index as -1 if index value is less than -1. Negative index values less than -1 are not valid.
            if (selectedIndex < -1)
            {
                return -1;
            }

            //// Considered the index value as the last item index value. Because the index value is not greater than the item source count.
            if (selectedIndex >= itemsCount)
            {
                return itemsCount - 1;
            }

            return selectedIndex;
        }

        /// <summary>
        /// Method to get the collection is equal or not.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <param name="newCollection">The new collection.</param>
        /// <returns>Returns collections are equal.</returns>
        internal static bool IsCollectionEquals(ObservableCollection<string> collection, ObservableCollection<string> newCollection)
        {
            if (collection == newCollection)
            {
                return true;
            }

            if (collection == null || newCollection == null)
            {
                return false;
            }

            if (collection.Count != newCollection.Count)
            {
                return false;
            }

            for (int index = 0; index < collection.Count; index++)
            {
                object item = collection[index];
                object newItem = newCollection[index];
                if (!item.Equals(newItem))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Method to create a template view.
        /// </summary>
        /// <param name="template">The data template.</param>
        /// <param name="itemDetail">The item details.</param>
        /// <returns>Returns the view from the view template</returns>
        internal static View CreateTemplateView(DataTemplate template, PickerItemDetails itemDetail)
        {
            View view;
            var content = template.CreateContent();
            if (content is ViewCell)
            {
                view = ((ViewCell)content).View;
            }
            else
            {
                view = (View)content;
            }

            if (view.BindingContext == null)
            {
                view.BindingContext = itemDetail;
            }

            return view;
        }

        /// <summary>
        /// Method to get the name based on parent.
        /// </summary>
        /// <param name="parent">The parent details.</param>
        /// <returns>Returns the name based on parent.</returns>
        internal static string GetParentName(Element parent)
        {
            string name = string.Empty;
            switch (parent)
            {
                case SfPicker:
                    name = "Picker";
                    break;
                case SfDatePicker:
                    name = "DatePicker";
                    break;
                case SfTimePicker:
                    name = "TimePicker";
                    break;
                case SfDateTimePicker:
                    name = "DateTimePicker";
                    break;
                default:
                    name = string.Empty;
                    break;
            }

            return name;
        }

        #endregion
    }
}