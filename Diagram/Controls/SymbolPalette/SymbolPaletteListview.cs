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

using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Diagram.DiagramModel;

namespace Diagram.Controls.SymbolPalette
{
    public sealed class SymbolPaletteListView : ListView
    {
        private const int LVM_FIRST = 0x1000; // ListView messages
        private const int LVM_SETGROUPINFO = (LVM_FIRST + 147); // ListView messages Setinfo on Group
        private const int WM_LBUTTONUP = 0x0202; // Windows message left button

        private delegate void CallBackSetGroupState(ListViewGroup lstvwgrp, ListViewGroupState state);

        private delegate void CallbackSetGroupString(ListViewGroup lstvwgrp, string value);


        [DllImport("User32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, IntPtr lParam);

        [DllImport("User32.dll")]
        private static extern int SendMessage(HandleRef hWnd, int Msg, int wParam, LVHITTESTINFO lParam);

        private readonly ListViewGroup favourites;
        private readonly ListViewItem dummy;

        private readonly ContextMenu favouritesMenu;
        private readonly MenuItem deleteFavouriteItem;

        public SymbolPaletteListView()
        {
            View = View.LargeIcon;
            //TileSize = new Size(80, 80);
            favourites = favourites = Groups.Add("favourites", "Favourites");
            SetGroupState(favourites, ListViewGroupState.Collapsible);
            dummy = new ListViewItem("", "empty");
            InsertDummy();
            AllowDrop = true;

            deleteFavouriteItem = new MenuItem
            {
                Name = "deleteFavourite",
                Text = "Delete Favourite"
            };
            deleteFavouriteItem.Click += DeleteFavourite;

            favouritesMenu = new ContextMenu();
            favouritesMenu.MenuItems.Add(deleteFavouriteItem);
            favouritesMenu.Name = "FavouritesMenu";
        }


        private void DeleteFavourite(object sender, EventArgs e)
        {
			if(favourites.Items.Count == 1 && favourites.Items[0] == FocusedItem)
				InsertDummy();
            Items.Remove(FocusedItem);
        }

        public void Add(PaletteObj symbol)
        {
            var group = Groups[symbol.Group];
            if(group == null)
            {
                group = Groups.Add(symbol.Group, symbol.Group);
                SetGroupState(group, ListViewGroupState.Collapsible);
            }
            CreateSymbol(symbol, group);
        }

        private void CreateSymbol(PaletteObj symbol, ListViewGroup group)
        {
            var item = Items.Add(symbol.Task.Symbol.Label, symbol.Image);
            item.Name = symbol.Task.Symbol.Name;
            item.Tag = symbol;
            item.Group = group;
        }

        protected override void WndProc(ref Message m)
        {
            if(m.Msg == WM_LBUTTONUP)
                DefWndProc(ref m);
            base.WndProc(ref m);
        }

        public ListViewGroup GroupHitTest(int x, int y)
        {
            if(View == View.Details || !ClientRectangle.Contains(x, y))
                return null;

            // TODO: Surely this needs to be marshalled?
            // Pinning using GCHandle does not work 
            // This code was taken from the dot net implementation of ListView so I guess it must be right
            var lParam = new LVHITTESTINFO {pt_x = x, pt_y = y};
			
            var index = SendMessage(new HandleRef(this, Handle), 4114, -1, lParam);

            return (LVHTFlags)lParam.flags == LVHTFlags.LVHT_EX_GROUP_BACKGROUND
                ? Groups.Cast<ListViewGroup>().SingleOrDefault(g => index == GetGroupId(g))
                : null;
        }


        private static int GetGroupId(ListViewGroup lstvwgrp)
        {
            var pi = lstvwgrp.GetType().GetProperty("ID", BindingFlags.NonPublic | BindingFlags.Instance);
            var tmprtnval = pi.GetValue(lstvwgrp, null);
            return (int)tmprtnval;

        }

        internal void SetGroupState(ListViewGroup lstvwgrp, ListViewGroupState state)
        {
            if(Environment.OSVersion.Version.Major < 6) //Only Vista and after
                return;
            if(lstvwgrp?.ListView == null)
                return;
            if(lstvwgrp.ListView.InvokeRequired)
                lstvwgrp.ListView.Invoke(new CallBackSetGroupState(SetGroupState), lstvwgrp, state);
            else
            {
                int id = GetGroupId(lstvwgrp);
                LVGROUP group = new LVGROUP();
                group.CbSize = Marshal.SizeOf(group);
                group.State = state;
                group.Mask = ListViewGroupMask.State;

                group.IGroupId = id;

				IntPtr ip = IntPtr.Zero;
				try
				{
					ip = Marshal.AllocHGlobal(group.CbSize);
					Marshal.StructureToPtr(group, ip, false);
					SendMessage(lstvwgrp.ListView.Handle, LVM_SETGROUPINFO, id, ip);
				}
				finally
				{
					Marshal.FreeHGlobal(ip);
				}

                lstvwgrp.ListView.Refresh();
            }
        }
        
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                if(FocusedItem.Bounds.Contains(e.Location) && FocusedItem.Group == favourites)
                {
                    favouritesMenu.Show(this, e.Location);
                }
            }
        }

        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            base.OnItemDrag(e);
            if(e.Button == MouseButtons.Left)
                DoDragDrop(e.Item, DragDropEffects.Copy | DragDropEffects.Scroll);
        }

        protected override void OnDragEnter(DragEventArgs e)
        {
            base.OnDragEnter(e);
            e.Effect = e.AllowedEffect;
        }

        protected override void OnDragOver(DragEventArgs e)
        {
            base.OnDragOver(e);
            e.Effect = DragDropEffects.None;
            var dragged = e.Data.GetData(typeof(ListViewItem)) as ListViewItem;

            if(dragged == null)
                return;

            if(favourites.Items.Cast<ListViewItem>().Any(i => i.Name == dragged.Name))
                return;

            var targetPoint = PointToClient(new Point(e.X, e.Y));
            var item = GetItemAt(targetPoint.X, targetPoint.Y);
            if(item == null)
            {
                var group = GroupHitTest(targetPoint.X, targetPoint.Y);
                if(group != null && group == favourites)
                    e.Effect = DragDropEffects.Copy;
            }
            else if(item == dummy)
            {
                e.Effect = DragDropEffects.Copy;
            }

            if(targetPoint.Y < 10)
            {
                Debug.WriteLine(e.Y);
                SendMessage(new HandleRef(this, Handle), 277, 0, null);
            }
            else if(targetPoint.Y > Height - 10)
            {
                Debug.WriteLine(e.Y);
                SendMessage(new HandleRef(this, Handle), 277, 1, null);
            }
        }

        protected override void OnDragDrop(DragEventArgs e)
        {
            base.OnDragDrop(e);
            var item = e.Data.GetData(typeof(ListViewItem)) as ListViewItem;
            if(!(item?.Tag is PaletteObj))
                return;

            RemoveDummy();

            CreateSymbol((PaletteObj)item.Tag, favourites);
        }

        protected override void OnItemSelectionChanged(ListViewItemSelectionChangedEventArgs e)
        {
            if(e.Item.Group != favourites)
                e.Item.Selected = false;
            base.OnItemSelectionChanged(e);
        }


        private void InsertDummy()
        {
            Items.Add(dummy);
            dummy.Group = favourites;
        }

        private void RemoveDummy()
        {
            var d = Items.IndexOf(dummy);
            if(d != -1) Items.RemoveAt(d);
        }
    }

    /// <summary>
    /// LVGROUP StructureUsed to set and retrieve groups.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct LVGROUP
    {
        public int CbSize;
        public ListViewGroupMask Mask;
        [MarshalAs(UnmanagedType.LPWStr)] 
        public string PszHeader;
        public int CchHeader;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string PszFooter;
        public int CchFooter;
        public int IGroupId;
        public int StateMask;
        public ListViewGroupState State;
        public uint UAlign;
        public IntPtr PszSubtitle;
        public uint CchSubtitle;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string PszTask;
        public uint CchTask;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string PszDescriptionTop;
        public uint CchDescriptionTop;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string PszDescriptionBottom;
        public uint CchDescriptionBottom;
        public int ITitleImage;
        public int IExtendedImage;
        public int IFirstItem;
        public IntPtr CItems;
        public IntPtr PszSubsetTitle;
        public IntPtr CchSubsetTitle;
    }

    public enum ListViewGroupMask
    {
        None = 0x00000,
        Header = 0x00001,
        Footer = 0x00002,
        State = 0x00004,
        Align = 0x00008,
        GroupId = 0x00010,
        SubTitle = 0x00100,
        Task = 0x00200,
        DescriptionTop = 0x00400,
        DescriptionBottom = 0x00800,
        TitleImage = 0x01000,
        ExtendedImage = 0x02000,
        Items = 0x04000,
        Subset = 0x08000,
        SubsetItems = 0x10000
    }

    public enum ListViewGroupState
    {
        Normal = 0,
        Collapsed = 1,
        Hidden = 2,
        NoHeader = 4,
        Collapsible = 8,
        Focused = 16,
        Selected = 32,
        SubSeted = 64,
        SubSetLinkFocused = 128,
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class LVHITTESTINFO
    {
        public int pt_x;
        public int pt_y;
        public int flags;
        public int iItem;
        public int iSubItem;
        public int iGroup;
    }

    [Flags]
    internal enum LVHTFlags : uint
    {
        LVHT_NOWHERE = 0x0001,
        LVHT_ONITEMICON = 0x0002,
        LVHT_ONITEMLABEL = 0x0004,
        LVHT_ONITEMSTATEICON = 0x0008,
        LVHT_ONITEM = (LVHT_ONITEMICON | LVHT_ONITEMLABEL | LVHT_ONITEMSTATEICON),
        LVHT_EX_GROUP_HEADER = 0x10000000,
        LVHT_EX_GROUP_FOOTER = 0x20000000,
        LVHT_EX_GROUP_COLLAPSE = 0x40000000,
        LVHT_EX_GROUP_BACKGROUND = 0x80000000,
        LVHT_EX_GROUP_STATEICON = 0x01000000,
        LVHT_EX_GROUP_SUBSETLINK = 0x02000000,
        LVHT_EX_GROUP =(LVHT_EX_GROUP_BACKGROUND | LVHT_EX_GROUP_COLLAPSE | LVHT_EX_GROUP_FOOTER | LVHT_EX_GROUP_HEADER | LVHT_EX_GROUP_STATEICON | LVHT_EX_GROUP_SUBSETLINK),
        LVHT_EX_ONCONTENTS = 0x04000000,
        LVHT_EX_FOOTER = 0x08000000,
    }
}
