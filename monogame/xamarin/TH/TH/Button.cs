using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;

namespace TH
{
	public class Button
	{
		public int YUp;
		public int YDown;
		public int XRight;
		public int XLeft;

		public int Dx;
		public int deltaY;

		public Texture2D TextureButton;
		public Texture2D TextureButtonLight;

		private TouchCollection Touches;
		private bool IsPressed;
		public bool IsEnabled;

		public Button()
		{
			init ();
		}
			
		public Button(Texture2D texture, Texture2D texture2)
		{
			TextureButton = texture;
			TextureButtonLight = texture2;
			init ();
		}

		public void init() {
			XRight = -100;
			XLeft = -100;
			YUp = -100;
			YDown = -100;
			IsPressed = false;
			IsEnabled = false;
			Dx = 1;
			deltaY = 0;
		}

		public void Update(int xLeft, int yUp)
		{
			XRight = XLeft+TextureButton.Width;
			XLeft = xLeft;
			YUp = yUp;
			YDown = yUp + TextureButton.Height;
		}

		public void Process(SpriteBatch spriteBatch)
		{
			Touches = TouchPanel.GetState();
			if (Touches.Count == 1)
			{
				if (!IsPressed)
				{
					spriteBatch.Draw(TextureButton, new Vector2(AbsoluteX(XLeft), AbsoluteY(YUp)),
						new Rectangle(0, 0, TextureButton.Width, TextureButton.Height), Color.White, 0,
						new Vector2(0, 0),
						Dx, SpriteEffects.None, 0);
				}
				if ((Touches[0].Position.X > AbsoluteX(XLeft)) && (Touches[0].Position.X < AbsoluteX(XRight+10)) &&
					(Touches[0].Position.Y > AbsoluteX(YUp)) && (Touches[0].Position.Y < AbsoluteY(YDown+10)))
				{
					spriteBatch.Draw(TextureButtonLight, new Vector2(AbsoluteX(XLeft), AbsoluteY(YUp)),
						new Rectangle(0, 0, TextureButtonLight.Width, TextureButtonLight.Height), Color.White, 0,
						new Vector2(0, 0),
						Dx, SpriteEffects.None, 0);
					IsPressed = true;
				}
				else
				{
					if (IsPressed) IsPressed = false;
				}
			}
			else
			{
				spriteBatch.Draw(TextureButton, new Vector2(AbsoluteX(XLeft), AbsoluteY(YUp)),
					new Rectangle(0, 0, TextureButton.Width, TextureButton.Height), Color.White, 0,
					new Vector2(0, 0),
					Dx, SpriteEffects.None, 0);
			}
			if ((IsPressed) && (Touches.Count == 0))
			{
				IsEnabled = true;
			}
		}
		public void Reset()
		{
			IsPressed = false;
			IsEnabled = false;
		}

		// коррекция координаты X
		public float AbsoluteX(float x)
		{
			return x*Dx;
		}

		// коррекция координаты Y
		public float AbsoluteY(float y)
		{
			return y*Dx + deltaY;
		}

	}
}

