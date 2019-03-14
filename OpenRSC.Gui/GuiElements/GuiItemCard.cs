﻿using Microsoft.Xna.Framework;

using NuciXNA.Graphics.Drawing;
using NuciXNA.Gui.GuiElements;
using NuciXNA.Primitives;

namespace OpenRSC.Gui.GuiElements
{
    public class GuiItemCard : GuiElement
    {
        const int SpriteRows = 32;
        const int SpriteColumns = 32;

        GuiImage icon;
        GuiText quantity;

        public int ItemPictureId { get; set; }

        public int Quantity { get; set; }

        public GuiItemCard()
        {
            Size = new Size2D(36, 36);
        }

        public override void LoadContent()
        {
            icon = new GuiImage
            {
                Size = new Size2D(32, 32),
                ContentFile = "Interface/items"
            };
            quantity = new GuiText
            {
                Size = new Size2D(Size.Width, 10),
                FontName = "ItemCardFont",
                FontOutline = FontOutline.BottomRight,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left
            };

            AddChild(icon);
            AddChild(quantity);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            icon.Visible = Quantity > 0;
            quantity.Visible = Quantity > 1;

            base.Update(gameTime);
        }

        protected override void SetChildrenProperties()
        {
            base.SetChildrenProperties();

            icon.SourceRectangle = CalculateIconSourceRectangle(ItemPictureId);
            icon.Location = new Point2D(
                Location.X + (Size.Width - icon.Size.Width) / 2,
                Location.Y + (Size.Height - icon.Size.Height) / 2);

            quantity.Location = Location;
            quantity.Text = Quantity.ToString();
        }

        Rectangle2D CalculateIconSourceRectangle(int id)
        {
            int x = id % SpriteColumns;
            int y = id / SpriteRows;

            return new Rectangle2D(x * icon.Size.Width,
                                   y * icon.Size.Height,
                                   icon.Size.Width,
                                   icon.Size.Height);
        }
    }
}
