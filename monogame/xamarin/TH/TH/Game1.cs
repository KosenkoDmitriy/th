#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

#endregion

namespace TH
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Button btn;

		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";
			graphics.IsFullScreen = false;
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{

			// TODO: Add your initialization logic here
			base.Initialize ();
				
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch (GraphicsDevice);

			playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
			//playerTexture = Content.Load<Texture2D> ("..\\Content\\cards\\10_clubs.jpg");//, playerPosition);
			playerTexture = Content.Load<Texture2D> ("cards\\10_clubs.jpg");//, playerPosition);
			//TODO: use this.Content to load your game content here
			var btnTexture = Content.Load<Texture2D> ("cards\\2_dia.jpg");
			var btnTextureLight = Content.Load<Texture2D> ("cards\\3_dia.jpg");
			btn = new Button (btnTexture, btnTextureLight);


		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			// Exit() is obsolete on iOS
			#if !__IOS__
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
			    Keyboard.GetState ().IsKeyDown (Keys.Escape)) {
				Exit ();
			}
			#endif
			// TODO: Add your update logic here		
			btn.Update (10, 10);
			base.Update (gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.CornflowerBlue);
		
			//TODO: Add your drawing code here
			spriteBatch.Begin ();
			spriteBatch.Draw(playerTexture, playerPosition, null, Color.White, 0f, Vector2.Zero, 1f,SpriteEffects.None, 0f);

			btn.Process (spriteBatch);
			spriteBatch.End ();
			base.Draw (gameTime);
		}
		public Vector2 playerPosition;
		public Texture2D playerTexture;


	}
}

