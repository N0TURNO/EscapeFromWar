using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using EscapeFromWar.GraphicsSupport;

namespace EscapeFromWar
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        #region // Variaveis globais.
        static public ContentManager content; // Processamento de imagens.
        static public GraphicsDeviceManager graphics; // Tamanho da janela.
        static public SpriteBatch spriteBatch; //Suporte para a os assets visuais.
        static public Random numAleatorios; // Para gerar numeros aleatorios.
        #endregion

        #region Tamanho da Janela (default)
        const int larguraJanela = 1080; // Largura da janela.
        const int alturaJanela = 720; // Altura da janela.
        #endregion

        public static float gravidade = 0.01f;

        EstadodeJogo meuJogo; 

        public Game1()// Inicializador da classe Game1.
        {
            // Controla o tamanho da janela.
            graphics = new GraphicsDeviceManager(this);
            // Processamento de imagem.
            Content.RootDirectory = "Content";
            Game1.content = Content;
            // Define o tamanho da imagem.
            Game1.graphics.PreferredBackBufferWidth = larguraJanela;
            Game1.graphics.PreferredBackBufferHeight = alturaJanela;
            // Gerador de numeros.
            Game1.numAleatorios = new Random();
           
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Cria uma nova SpriteBach para carregar iamgens.
            Game1.spriteBatch = new SpriteBatch(GraphicsDevice);
            // Define o limite da camera.
            Camera.DefineCamera(new Vector2(0f, 0f), 100f);

            meuJogo = new EstadodeJogo();
           
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // Allows the game to exit
            if (InputWrapper.Buttons.Back == ButtonState.Pressed)
                this.Exit();
            meuJogo.UpdateJogo(gameTime);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Game1.spriteBatch.Begin();
            //Comecar a fazer redering

            meuJogo.DrawJogo();

            Game1.spriteBatch.End();
            //Acabar de fazer rendering


            base.Draw(gameTime);
        }
    }
}
