﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Automation.Peers;
using Microsoft.UI.Xaml.Automation.Provider;
using Microsoft.UI.Xaml.Media;
using Syncfusion.Maui.Toolkit.Platform;
using Syncfusion.Maui.Toolkit.Graphics.Internals;

namespace Syncfusion.Maui.Toolkit.Semantics
{

	internal partial class CustomAutomationPeer : FrameworkElementAutomationPeer
	{
		/// <summary>
		/// Holds the information for the semantics nodes.
		/// </summary>
		private readonly SfView _mauiView;

		/// <summary>
		/// Holds the semantics nodes for the view.
		/// </summary>
		private List<SemanticsNode>? _semanticNodes;

		/// <summary>
		/// Holds the automation peers for the semantics nodes.
		/// </summary>
		private readonly IList<AutomationPeer> _automationPeers;

		internal CustomAutomationPeer(NativeGraphicsView owner, SfView mauiView) : base(owner)
		{
			_mauiView = mauiView;
			_automationPeers = [];
		}

		/// <summary>
		/// Return the class name and the value added to name value while hovering. 
		/// </summary>
		/// <returns>The class name.</returns>
		protected override string GetClassNameCore()
		{
			return "";
		}

		/// <summary>
		/// Return the control type. Here, this class used to hold list of children automation peer.
		/// </summary>
		/// <returns>The control type.</returns>
		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Custom;
		}

		/// <summary>
		/// Information of the automation peer.
		/// </summary>
		/// <returns>Name of automation peer.</returns>
		protected override string GetNameCore()
		{
			return "";
		}

		/// <summary>
		/// Invalidate the children automation peer.
		/// </summary>
		internal void InvalidateSemantics()
		{
			GetChildren().Clear();
		}

		/// <summary>
		/// Return the children of the automation peer.
		/// </summary>
		/// <returns>The children.</returns>
		protected override IList<AutomationPeer> GetChildrenCore()
		{
			_automationPeers.Clear();
			_semanticNodes = ((ISemanticsProvider)_mauiView).GetSemanticsNodes(_mauiView.Width, _mauiView.Height);
			AutomationPeer? previousAutomationPeer = null;
			SemanticsNode? previousNode = null;
			if (_semanticNodes != null)
			{
				for (int i = 0; i < _semanticNodes.Count; i++)
				{
					SemanticsNode semanticsNode = _semanticNodes[i];
					//// Convert the node position into screen position.
					AutomationPeer peer;
					if (semanticsNode.IsTouchEnabled)
					{
						peer = new CustomButtonAutomationPeer((FrameworkElement)Owner, semanticsNode, _mauiView);
					}
					else
					{
						peer = new CustomTextAutomationPeer((FrameworkElement)Owner, semanticsNode, _mauiView);
					}

					if (previousAutomationPeer != null && previousAutomationPeer is ICustomAutomationPeer peer2)
					{
						peer2.NextSibling = peer;
						peer2.NextNode = semanticsNode;
					}

					if (peer is ICustomAutomationPeer peer1)
					{
						peer1.PrevSibling = previousAutomationPeer;
						peer1.PrevNode = previousNode;
					}

					previousAutomationPeer = peer;
					previousNode = semanticsNode;
					_automationPeers.Add(peer);
				}
			}

			return _automationPeers;
		}

		/// <summary>
		/// Handles and return the automation peer while navigation.
		/// </summary>
		/// <param name="direction">Action performed on automation peer.</param>
		/// <returns>Return the automation peer.</returns>
		protected override object NavigateCore(AutomationNavigationDirection direction)
		{
			if (direction == AutomationNavigationDirection.FirstChild && _automationPeers != null && _automationPeers.Count > 0)
			{
				if (_semanticNodes != null && _semanticNodes.Count > 0)
				{
					//// Perform scroll while the view placed inside the scroll view and the current
					//// child is not in visible region.
					((ISemanticsProvider)_mauiView).ScrollTo(_semanticNodes[0]);
				}

				return _automationPeers[0];
			}
			else if (direction == AutomationNavigationDirection.LastChild && _automationPeers != null && _automationPeers.Count > 0)
			{
				if (_semanticNodes != null && _semanticNodes.Count > 0)
				{
					//// Perform scroll while the view placed inside the scroll view and the current
					//// child is not in visible region.
					((ISemanticsProvider)_mauiView).ScrollTo(_semanticNodes[^1]);
				}

				return _automationPeers[_automationPeers.Count - 1];
			}

			return base.NavigateCore(direction);
		}

		/// <summary>
		/// Return the automation peer based on its position.
		/// </summary>
		/// <param name="pointInWindowCoordinates">The interacted point.</param>
		/// <returns>The automation peer.</returns>
		protected override object GetElementFromPointCore(Windows.Foundation.Point pointInWindowCoordinates)
		{
			if (_semanticNodes != null)
			{
				NativeGraphicsView owner = (NativeGraphicsView)Owner;
				double scale = owner.XamlRoot.RasterizationScale;
				Point viewStartPosition = AccessibilityHelper.GetViewStartPosition(owner, scale);

				for (int i = 0; i < _automationPeers.Count; i++)
				{
					ICustomAutomationPeer automationPeer = (ICustomAutomationPeer)_automationPeers[i];
					Rect semanticNodeBounds = new Rect(automationPeer.Node.Bounds.Left * scale, automationPeer.Node.Bounds.Top * scale, automationPeer.Node.Bounds.Width * scale, automationPeer.Node.Bounds.Height * scale);
					Rect bounds = new Rect(viewStartPosition.X + semanticNodeBounds.Left, viewStartPosition.Y + semanticNodeBounds.Top, semanticNodeBounds.Width, semanticNodeBounds.Height);
					if (bounds.Contains(new Point(pointInWindowCoordinates.X, pointInWindowCoordinates.Y)))
					{
						return automationPeer;
					}
				}
			}

			return base.GetElementFromPointCore(pointInWindowCoordinates);
		}
	}

	/// <summary>
	/// Interface that used to hold its siblings
	/// </summary>
	internal interface ICustomAutomationPeer
	{
		/// <summary>
		/// Holds the previous sibling of the automation peer.
		/// </summary>
		AutomationPeer? PrevSibling { get; set; }

		/// <summary>
		/// Holds the previous sibling node information.
		/// </summary>
		SemanticsNode? PrevNode { get; set; }

		/// <summary>
		/// Hold the next sibling if the automation peer.
		/// </summary>
		AutomationPeer? NextSibling { get; set; }

		/// <summary>
		/// Holds the next sibling node information.
		/// </summary>
		SemanticsNode? NextNode { get; set; }

		/// <summary>
		/// Used to get the node information.
		/// </summary>
		SemanticsNode Node { get; }

		/// <summary>
		/// Holds the parent SfView instance for scroll action.
		/// </summary>
		SfView MauiSfView { get; set; }
	}

	/// <summary>
	/// Text automation peer.
	/// Custom text automation class created because label instance needed while create framework automation peer.
	/// </summary>
	internal partial class CustomTextAutomationPeer : FrameworkElementAutomationPeer, ICustomAutomationPeer
	{
		/// <summary>
		/// The information for the automation peer.
		/// </summary>
		private readonly SemanticsNode _semanticsNode;

		internal CustomTextAutomationPeer(FrameworkElement owner, SemanticsNode semanticsNode, SfView mauiView) : base(owner)
		{
			_semanticsNode = semanticsNode;
			MauiSfView = mauiView;
		}

		/// <summary>
		/// Holds the previous sibling of the automation peer.
		/// </summary>
		public AutomationPeer? PrevSibling { get; set; }

		/// <summary>
		/// Holds the next sibling of the automation peer.
		/// </summary>
		public AutomationPeer? NextSibling { get; set; }

		/// <summary>
		/// Holds the next sibling node information.
		/// </summary>
		public SemanticsNode? NextNode { get; set; }

		/// <summary>
		/// Holds the previous sibling node information.
		/// </summary>
		public SemanticsNode? PrevNode { get; set; }

		/// <summary>
		/// Gets the node information.
		/// </summary>
		public SemanticsNode Node => _semanticsNode;

		/// <summary>
		/// Holds the parent SfView instance for scroll action.
		/// </summary>
		public SfView MauiSfView { get; set; }

		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Text;
		}

		protected override string GetClassNameCore()
		{
			return "";
		}

		/// <summary>
		/// Return the information that needed on hovering.
		/// </summary>
		/// <returns>The name information.</returns>
		protected override string GetNameCore()
		{
			return _semanticsNode.Text;
		}

		/// <summary>
		/// Handles and return the automation peer while navigation.
		/// </summary>
		/// <param name="direction">Action performed on automation peer.</param>
		/// <returns>Return the automation peer.</returns>
		protected override object NavigateCore(AutomationNavigationDirection direction)
		{
			if (direction == AutomationNavigationDirection.NextSibling && NextSibling != null)
			{
				if (NextNode != null)
				{
					//// Perform scroll while the view placed inside the scroll view and the current
					//// child is not in visible region.
					((ISemanticsProvider)MauiSfView).ScrollTo(NextNode);
				}

				return NextSibling;
			}
			else if (direction == AutomationNavigationDirection.PreviousSibling && PrevSibling != null)
			{
				if (PrevNode != null)
				{
					//// Perform scroll while the view placed inside the scroll view and the current
					//// child is not in visible region.
					((ISemanticsProvider)MauiSfView).ScrollTo(PrevNode);
				}

				return PrevSibling;
			}

			return base.NavigateCore(direction);
		}

		/// <summary>
		/// Return the current automation peer bounds value related to screen.
		/// </summary>
		/// <returns>The rectangle bounds value.</returns>
		protected override Windows.Foundation.Rect GetBoundingRectangleCore()
		{
			Windows.Foundation.Rect rect = GetParent().GetBoundingRectangle();
			NativeGraphicsView owner = (NativeGraphicsView)Owner;
			double scale = owner.XamlRoot.RasterizationScale;
			Point viewStartPosition = AccessibilityHelper.GetViewStartPosition(owner, scale);
			Rect semanticNodeBounds = new Rect(_semanticsNode.Bounds.Left * scale, _semanticsNode.Bounds.Top * scale, _semanticsNode.Bounds.Width * scale, _semanticsNode.Bounds.Height * scale);
			Rect bounds = new Rect(viewStartPosition.X + semanticNodeBounds.Left, viewStartPosition.Y + semanticNodeBounds.Top, semanticNodeBounds.Width, semanticNodeBounds.Height);
			double yPosition = bounds.Y;
			double endYPosition = bounds.Y + bounds.Height;

			double xPosition = bounds.X;
			double endXPosition = bounds.X + bounds.Width;

			if (yPosition < rect.Top)
			{
				yPosition = rect.Top;
			}

			if (endYPosition > rect.Top + rect.Height)
			{
				endYPosition = rect.Top + rect.Height;
			}

			if (xPosition < rect.Left)
			{
				xPosition = rect.Left;
			}

			if (endXPosition > rect.Left + rect.Width)
			{
				endXPosition = rect.Left + rect.Width;
			}

			double width = endXPosition - xPosition;
			if (width < 0)
			{
				width = 0;
			}

			double height = endYPosition - yPosition;
			if (height < 0)
			{
				height = 0;
			}

#if WINDOWS
			//// If the SfView drawing canvas size exceeds MaximumBitmapSizeInPixels when adding more items,
			//// an OS limitation with CanvasImageSource size (refer: https://github.com/dotnet/maui/issues/3785)
			//// requires restricting the draw function when semantics or accessibility are used on SfView. This prevents OS limitation issues on the Windows platform.
			//// For accessibility, SfView should be enabled with AboveContentWithTouch to add a native user control and override the AutomationPeer.
			//// Virtualization isn't possible because automation peers must be added initially to access scrollable content. 
			//// Since accessibility highlights are managed by native framework automation peers, SfView canvas drawing is unnecessary.
			if (!MauiSfView.IsCanvasNeeded)
			{
				return new Windows.Foundation.Rect(xPosition, yPosition, semanticNodeBounds.Width, semanticNodeBounds.Height);
			}
#endif

			return new Windows.Foundation.Rect(xPosition, yPosition, width, height);
		}
	}

	/// <summary>
	/// Button automation peer.
	/// Custom button automation class created because button instance needed while create framework automation peer.
	/// </summary>
	internal partial class CustomButtonAutomationPeer : FrameworkElementAutomationPeer, ICustomAutomationPeer, IInvokeProvider
	{
		/// <summary>
		/// The information for the automation peer.
		/// </summary>
		private readonly SemanticsNode _semanticsNode;

		internal CustomButtonAutomationPeer(FrameworkElement owner, SemanticsNode semanticsNode, SfView mauiView) : base(owner)
		{
			_semanticsNode = semanticsNode;
			MauiSfView = mauiView;
		}

		/// <summary>
		/// Holds the previous sibling of the automation peer.
		/// </summary>
		public AutomationPeer? PrevSibling { get; set; }

		/// <summary>
		/// Holds the previous sibling of the automation peer.
		/// </summary>
		public AutomationPeer? NextSibling { get; set; }

		/// <summary>
		/// Holds the next sibling node information.
		/// </summary>
		public SemanticsNode? NextNode { get; set; }

		/// <summary>
		/// Holds the previous sibling node information.
		/// </summary>
		public SemanticsNode? PrevNode { get; set; }

		/// <summary>
		/// Gets the node information.
		/// </summary>
		public SemanticsNode Node => _semanticsNode;

		/// <summary>
		/// Holds the parent SfView instance for scroll action.
		/// </summary>
		public SfView MauiSfView { get; set; }

		/// <summary>
		/// Handles and return the automation peer while navigation.
		/// </summary>
		/// <param name="direction">Action performed on automation peer.</param>
		/// <returns>Return the automation peer.</returns>
		protected override object NavigateCore(AutomationNavigationDirection direction)
		{
			if (direction == AutomationNavigationDirection.NextSibling && NextSibling != null)
			{
				if (NextNode != null)
				{
					//// Perform scroll while the view placed inside the scroll view and the current
					//// child is not in visible region.
					((ISemanticsProvider)MauiSfView).ScrollTo(NextNode);
				}

				return NextSibling;
			}
			else if (direction == AutomationNavigationDirection.PreviousSibling && PrevSibling != null)
			{
				if (PrevNode != null)
				{
					//// Perform scroll while the view placed inside the scroll view and the current
					//// child is not in visible region.
					((ISemanticsProvider)MauiSfView).ScrollTo(PrevNode);
				}

				return PrevSibling;
			}

			return base.NavigateCore(direction);
		}

		protected override AutomationControlType GetAutomationControlTypeCore()
		{
			return AutomationControlType.Button;
		}

		/// <summary>
		/// Decide to handle the actions that related to this automation peer.
		/// </summary>
		/// <param name="patternInterface">Performed action.</param>
		/// <returns>The automation peer.</returns>
		protected override object GetPatternCore(PatternInterface patternInterface)
		{
			if (patternInterface == PatternInterface.Invoke)
			{
				return this;
			}

			return base.GetPatternCore(patternInterface);
		}

		/// <summary>
		/// Return the information that needed on hovering.
		/// </summary>
		/// <returns>The name information.</returns>
		protected override string GetNameCore()
		{
			return _semanticsNode.Text;
		}

		/// <summary>
		/// Return the current automation peer bounds value related to screen.
		/// </summary>
		/// <returns>The rectangle bounds value.</returns>
		protected override Windows.Foundation.Rect GetBoundingRectangleCore()
		{
			Windows.Foundation.Rect rect = GetParent().GetBoundingRectangle();
			NativeGraphicsView owner = (NativeGraphicsView)Owner;
			double scale = owner.XamlRoot.RasterizationScale;
			Point viewStartPosition = AccessibilityHelper.GetViewStartPosition(owner, scale);
			Rect semanticNodeBounds = new Rect(_semanticsNode.Bounds.Left * scale, _semanticsNode.Bounds.Top * scale, _semanticsNode.Bounds.Width * scale, _semanticsNode.Bounds.Height * scale);
			Rect bounds = new Rect(viewStartPosition.X + semanticNodeBounds.Left, viewStartPosition.Y + semanticNodeBounds.Top, semanticNodeBounds.Width, semanticNodeBounds.Height);
			double yPosition = bounds.Y;
			double endYPosition = bounds.Y + bounds.Height;

			double xPosition = bounds.X;
			double endXPosition = bounds.X + bounds.Width;

			if (yPosition < rect.Top)
			{
				yPosition = rect.Top;
			}

			if (endYPosition > rect.Top + rect.Height)
			{
				endYPosition = rect.Top + rect.Height;
			}

			if (xPosition < rect.Left)
			{
				xPosition = rect.Left;
			}

			if (endXPosition > rect.Left + rect.Width)
			{
				endXPosition = rect.Left + rect.Width;
			}

			double width = endXPosition - xPosition;
			if (width < 0)
			{
				width = 0;
			}

			double height = endYPosition - yPosition;
			if (height < 0)
			{
				height = 0;
			}

			return new Windows.Foundation.Rect(xPosition, yPosition, width, height);
		}

		/// <summary>
		/// Sends a request to initiate or perform the invoke action of the provider control.
		/// </summary>
		void IInvokeProvider.Invoke()
		{
			if (_semanticsNode.OnClick == null)
			{
				return;
			}

			_semanticsNode.OnClick(_semanticsNode);
		}
	}

	/// <summary>
	/// Holds the methods that used for accessibility implementation.
	/// </summary>
	internal static class AccessibilityHelper
	{
		/// <summary>
		/// Return the view start position form screen.
		/// </summary>
		/// <param name="element">Element or layout that needed to calculate the start position.</param>
		/// <param name="scale">Scale value of the screen.</param>
		/// <returns></returns>
		internal static Point GetViewStartPosition(UIElement element, double scale)
		{
			//// Specifying null consider as root element(application window).
			GeneralTransform transform = element.TransformToVisual(null);
			//// Convert the layout point(0,0) into screen position.
			Windows.Foundation.Point startPoint = transform.TransformPoint(new Windows.Foundation.Point(0, 0));
			//// return the value based on the scale value.
			return new Point(startPoint.X * scale, startPoint.Y * scale);
		}
	}
}