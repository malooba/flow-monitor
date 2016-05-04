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

using System.Windows.Forms;

namespace FlowMonitor.ViewModules
{
    /// <summary>
    /// Base class of modules that can generate a UI
    /// </summary>
    internal abstract class ViewModule
    {
        /// <summary>
        /// The content for the left hand panel
        /// </summary>
        public Control SelectorControl { get; protected set; }

        /// <summary>
        /// The content for the middle panel
        /// </summary>
        public Control MainControl { get; protected set; }

        /// <summary>
        /// The content for the right hand panel
        /// </summary>
        public Control DetailControl { get; protected set; }

        /// <summary>
        /// Additional control toolbar for view
        /// </summary>
        public Control ToolBar { get; protected set; }

        /// <summary>
        /// Called when the user is trying to navigate away from this module
        /// </summary>
        /// <returns>True if the navigation should be permitted</returns>
        public virtual bool ViewModuleChanging()
        {
            return true;
        }

        /// <summary>
        /// Called when the view module has been selected and its panels have been installed
        /// </summary>
        public abstract void ViewModuleSelected();
    }
}
