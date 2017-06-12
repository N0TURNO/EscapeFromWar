using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using EscapeFromWar.GraphicsSupport;

namespace EscapeFromWar.GraphicsSupport
{
   public class Heroi:TexturasSprite
    {

        #region particulas

        //private const float tamanhoParticulas = 4f;
        //private const float tempoParticulas = 20f;

        //SistemaParticulas efeitoColisao = new SistemaParticulas();
        //private Particula CriaParticula(Vector2 posicao)
        //{
        //    return new Particula(posicao, tamanhoParticulas, tempoParticulas);
        //}

        #endregion

        private const float kLarguraHeroi = 3f;
        private const float ktempoEntreDisparos = 1.5f;
        private const float kDisparoFora = 0.35f * kLarguraHeroi;
        private const float tempoStunnrd = 1.5f;
        private const int kVidaHeroi = 2;
        private float tempoUltimoDisparo = 0;
        private float tempostunned;
        private int vidaHeroiActual;

        public int VidaHeroi
        {
            get { return vidaHeroiActual; }
            set
            {
                vidaHeroiActual = value;
                //this.Tamanho = new Vector2((kLarguraHeroi + kLarguraHeroi * (vidaHeroiActual - 1) / 3), kLarguraHeroi + kLarguraHeroi * (vidaHeroiActual - 1) / 3);
            }
        }

        private List<Shooting> disparos;
        public List<Shooting> todosDisparos() { return disparos; }
        private enum EstadoHeroi
        {
            Andar, Stealth, Morto, Stunned 
        }
        private EstadoHeroi estadoActual;

        public Heroi(Vector2 posicao) : base("p", posicao, new Vector2(kLarguraHeroi, kLarguraHeroi), 4, 2, 0)
        {
            //vidaHeroiActual = 1;
            tempostunned = 0;
            estadoActual = EstadoHeroi.Andar;
            disparos = new List<Shooting>();

            DefineAnimacao(0, 0, 0, 3, 10);
            SpriteLinhaActual = 0;
        }

        public void Update(GameTime gametime, Vector2 delta, bool houveDisparo)
        {
            switch(estadoActual)
            {
                case EstadoHeroi.Andar:
                    UpdateEstadoHeroi(gametime, delta, houveDisparo);
                    AudioSupport.PlaySom("walk fast");

                    break;
            }
        }
        public void UpdateEstadoHeroi(GameTime gametime, Vector2 delta, bool houveDisparo)
        {
            base.Update();
            boundToCamera();
            //Controlo jogador
            Posicao += delta/2;

            //direcao do sprite
            if (delta.X > 0)
                SpriteLinhaActual = 1;
            else if(delta.X<0)
                SpriteLinhaActual = 0;

            //Direcao do disparo
            int direcaoDisparo = 1;
            if (SpriteLinhaActual == 0)
                direcaoDisparo = -1;

            

            float deltaTime = gametime.ElapsedGameTime.Milliseconds;
            tempoUltimoDisparo += deltaTime / 1000;

            //verifica se pode fazer disparo
            if (tempoUltimoDisparo >= ktempoEntreDisparos)
            {
                if (houveDisparo)
                {
                    Shooting j = new Shooting(new Vector2(Posicao.X + kDisparoFora * direcaoDisparo, Posicao.Y), direcaoDisparo);
                    disparos.Add(j);
                    tempoUltimoDisparo = 0;
                    AudioSupport.PlaySom("gun");
                }
            }
            //update de todos os disparos
            int count = disparos.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                if (!disparos[i].BalaEstaNoEcra())
                {
                    disparos.RemoveAt(i);
                }
                else
                    disparos[i].Update();
            }
        }
        public void UpdateEstadoStunned(GameTime gametime)
        {
            float deltaTime = gametime.ElapsedGameTime.Milliseconds;
            tempostunned += deltaTime / 1000;
            if(tempostunned>=tempoStunnrd)
            {
                tempostunned = 0;
                estadoActual = EstadoHeroi.Morto;//Unnstunable
            }
        }
        public void UpdateUnnstunable(GameTime gametime)
        {
            float deltaTime = gametime.ElapsedGameTime.Milliseconds;
            tempostunned += deltaTime / 1000;
            if(tempostunned>=tempoStunnrd)
            {
                tempostunned = 0;
                estadoActual = EstadoHeroi.Andar;
            }
        }
        public override void Draw()
        {
            base.Draw();
            foreach (var j in disparos)
                j.Draw();
           // efeitoColisao.DrawSistemaParticulas();
        }

        public void AjustaVida(int ajuste)
        {
            if (ajuste + VidaHeroi > kVidaHeroi)
                return;
            VidaHeroi += ajuste;
            MathHelper.Clamp(VidaHeroi, 0, 3);
                if(vidaHeroiActual<=0)
            {
                estadoActual = EstadoHeroi.Morto;
            }
        }
        public void StealthHeroi()//Era o Unstunnable
        {
            if(estadoActual!=EstadoHeroi.Morto && estadoActual!=EstadoHeroi.Stunned)
            {
                estadoActual = EstadoHeroi.Stunned;
                AudioSupport.PlaySom("Stun");
                AjustaVida(-1);
            }
        }
        public bool Morto()
        {
            if (estadoActual == EstadoHeroi.Morto)
            {
                return true;
                AudioSupport.Background("game over_sound 1", 5f);
            }
            else
                return false;
        }

        public void AumentoVida()
            {
            AjustaVida(2);
            AudioSupport.PlaySom("Ration");
        }

    }

    
}
