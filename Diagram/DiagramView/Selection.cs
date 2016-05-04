//Copyright 2016 Malooba Ltd

//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at

//    http://www.apache.org/licenses/LICENSE-2.0

//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using Diagram.DiagramModel;

namespace Diagram.DiagramView
{
    /// <summary>
    /// The currently selected IVisibles
    /// </summary>
    public class Selection
    {
        private readonly List<Visible> items;
        private readonly View view;

        public Selection(View view)
        {
            this.view = view;
            items = new List<Visible>();
        }

        /// <summary>
        /// Easy way to get single selection if there is only one
        /// return null if there are more than one
        /// </summary>
        public Visible Single => items.Count == 1 ? items[0] : null;

        /// <summary>
        /// Get all items 
        /// </summary>
        public IEnumerable<Visible> Items => new ReadOnlyCollection<Visible>(items);

        public int Count => items.Count;

        // Return the bounds of all selected items
        public Rectangle Bounds => items.Aggregate(Rectangle.Empty, (current, item) => Rectangle.Union(current, item.Bounds));

       
        /// <summary>
        /// Clear all selections
        /// </summary>
        public void Clear()
        {
            var copy = items.ToArray();
            items.Clear();
            UpdateSelected(copy);
            UpdatePropertyEditor();
        }

        /// <summary>
        /// Replace selction with a single item
        /// </summary>
        /// <param name="item"></param>
        public void Select(Visible item)
        {
            var copy = items.ToArray();
            items.Clear();
            items.Add(item);
            UpdateSelected(copy);
            UpdatePropertyEditor();
        }

        /// <summary>
        /// Replace selction with a collection of items 
        /// </summary>
        /// <param name="item"></param>
        public void Select(IEnumerable<Visible> item)
        {
            var copy = items.ToArray();
            items.Clear();
            items.AddRange(item);
            UpdateSelected(copy);
            UpdatePropertyEditor();
        }

        /// <summary>
        /// Toggle the selection status of an item (ctrl-click)
        /// </summary>
        /// <param name="item"></param>
        public void Toggle(Visible item)
        {
            var copy = items.ToArray();
            if(items.Contains(item))
                items.Remove(item);
            else
                items.Add(item);
            UpdateSelected(copy);
            UpdatePropertyEditor();
        }

        public void SelectInRectangle(Rectangle frame)
        {
            var copy = items.ToArray();
            items.Clear();

            foreach (var task in Model.Workflow.Tasks.Where(task => frame.Contains(task.Symbol.Shape.Bounds)))
                items.Add(task.Symbol.Shape);

            foreach(var connector in Connector.Connectors.Where(conn => frame.Contains(conn.Bounds)))
                items.Add(connector);

            UpdateSelected(copy);
            UpdatePropertyEditor();
        }

        /// <summary>
        /// Check if an item is selected
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool IsSelected(Visible item)
        {
            return items.Contains(item);
        }

        /// <summary>
        /// Inform IVisibles that their selection status has changed
        /// Note that IVisibles do not hold a selection state so we don't send them one
        /// They refer to this object to get their status
        /// </summary>
        /// <param name="previous"></param>
        private void UpdateSelected(IEnumerable<Visible> previous)
        {
            // items being selected
            foreach(var item in items.Where(i => !previous.Contains(i)))
                item.SelectedChanged();

            // items being deselected
            foreach(var item in previous.Where(i => !items.Contains(i)))
                item.SelectedChanged();
        }

        /// <summary>
        /// Set the currently inspected object in the property editor
        /// </summary>
        private void UpdatePropertyEditor()
        {
            if(items.Count == 1 && items[0] is Shape)
                // Only show shape properties if one shape is selected
                view.ShowProperties(((Shape)items[0]).Task);
            else
                // Show the workflow if nothing is selected
                view.ShowProperties(null, items.Count == 0);
        }
    }
}
