﻿namespace AngleSharp.DOM.Html
{
    using AngleSharp.DOM.Collections;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the select element.
    /// </summary>
    [DomName("HTMLSelectElement")]
    public sealed class HTMLSelectElement : HTMLFormControlElementWithState
    {
        #region Fields

        HTMLCollection<HTMLOptionElement> _options;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new HTML select element.
        /// </summary>
        internal HTMLSelectElement()
        {
            _name = Tags.Select;
            _options = new HTMLCollection<HTMLOptionElement>(this);
            WillValidate = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets if the field is required.
        /// </summary>
        [DomName("required")]
        public Boolean Required
        {
            get { return GetAttribute(AttributeNames.Required) != null; }
            set { SetAttribute(AttributeNames.Required, value ? String.Empty : null); }
        }

        /// <summary>
        /// Gets the set of options that are selected.
        /// </summary>
        [DomName("selectedOptions")]
        public IHtmlCollection SelectedOptions
        {
            get 
            {
                var result = new List<HTMLOptionElement>();

                foreach (var option in _options)
                    if (option.Selected)
                        result.Add(option);

                return new HTMLCollection<HTMLOptionElement>(result);
            }
        }

        /// <summary>
        /// Gets the index of the first selected option element.
        /// </summary>
        [DomName("selectedIndex")]
        public Int32 SelectedIndex
        {
            get 
            { 
                var index = 0;

                foreach (var option in _options)
                {
                    if (option.Selected)
                        return index;

                    index++;
                }

                return -1;
            }
        }

        /// <summary>
        /// Gets or sets the value of this form control, that is, of the first selected option.
        /// </summary>
        [DomName("value")]
        public String Value
        {
            get
            {
                foreach (var option in _options)
                {
                    if (option.Selected)
                        return option.Value;
                }

                return null;
            }
            set
            {
                foreach (var option in _options)
                    option.Selected = option.Value == value;
            }
        }

        /// <summary>
        /// Gets the number of option elements in this select element.
        /// </summary>
        [DomName("length")]
        public Int32 Length
        {
            get { return Options.Length; }
        }

        /// <summary>
        /// Gets the multiple HTML attribute, whichindicates whether multiple items can be selected.
        /// </summary>
        [DomName("multiple")]
        public Boolean Multiple
        {
            get { return GetAttribute(AttributeNames.Multiple) != null; }
            set { SetAttribute(AttributeNames.Multiple, value ? String.Empty : null); }
        }

        /// <summary>
        /// Gets the set of option elements contained by this element. 
        /// </summary>
        [DomName("options")]
        public IHtmlCollection Options
        {
            get { return _options; }
        }

        /// <summary>
        /// Gets the form control's type.
        /// </summary>
        [DomName("type")]
        public SelectType Type
        {
            get { return Multiple ? SelectType.SelectMultiple : SelectType.SelectOne; }
        }

        #endregion

        #region Internal properties

        /// <summary>
        /// Gets if the node is in the special category.
        /// </summary>
        protected internal override Boolean IsSpecial
        {
            get { return true; }
        }

        #endregion

        #region Enumeration

        /// <summary>
        /// An enumeration with possible select types.
        /// </summary>
        public enum SelectType : ushort
        {
            /// <summary>
            /// Only one element can be selected.
            /// </summary>
            SelectOne,
            /// <summary>
            /// Multiple elements can be selected.
            /// </summary>
            SelectMultiple
        }

        #endregion

        #region Helpers

        internal override void ConstructDataSet(FormDataSet dataSet, HTMLElement submitter)
        {
            foreach (var option in _options)
            {
                if (option.Selected && !option.Disabled)
                    dataSet.Append(Name, option.Value, Multiple ? "select-one" : "select-multiple");
            }
        }

        /// <summary>
        /// Entry point for attributes to notify about a change (modified, added, removed).
        /// </summary>
        /// <param name="name">The name of the attribute that has been changed.</param>
        internal override void OnAttributeChanged(String name)
        {
            if (name.Equals(AttributeNames.Value, StringComparison.Ordinal))
                Value = GetAttribute(AttributeNames.Value);
            else
                base.OnAttributeChanged(name);
        }

        /// <summary>
        /// Resets the form control to its initial value.
        /// </summary>
        internal override void Reset()
        {
            foreach (var option in _options)
                option.Selected = option.DefaultSelected;
        }

        /// <summary>
        /// Checks the form control for validity.
        /// </summary>
        /// <param name="state">The element's validity state tracker.</param>
        protected override void Check(IValidityState state)
        {
            //TODO
        }

        #endregion
    }
}
