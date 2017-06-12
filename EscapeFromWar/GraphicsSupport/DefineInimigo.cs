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
    public enum TipoEnimigo
    {
        SoldadoExterno, SoldadoInterno
    }
    public class DefineInimigo
    {

        #region Particulas
        //private const float kCollideParticleSize = 3f;
        //private const int kCollideParticleLife = 80;
        //private static ParticleSystem sCollisionEffect = new ParticleSystem();
        //// to support particle system
        //static private ParticlePrimitive CreateRedParticle(Vector2 pos)
        //{
        //    return new ParticlePrimitive(pos, kCollideParticleSize, kCollideParticleLife);
        //}
        //static private ParticlePrimitive CreateDarkParticle(Vector2 pos)
        //{
        //    return new DarkParticlePrimitive(pos, kCollideParticleSize, kCollideParticleLife);
        //}


        #endregion
        private List<InimigoPatrulha> listaInimigos=new List<InimigoPatrulha>();
        private float adicionaDistanciaInimigo = 100f;

        private const int kNumeroInimigos = 5;

        public void DefineTipoEnimigo()
        {
            for(int i=0;i<kNumeroInimigos;i++)
            {
                InimigoPatrulha inimigo = SpawnInimigos();
                listaInimigos.Add(inimigo);

            }
        }

        public InimigoPatrulha SpawnInimigos()
        {
            int numAleatorio = (int)(Game1.numAleatorios.NextDouble() * 3);
            InimigoPatrulha inimigo = null;
            switch(numAleatorio)
            {
                //case (int)TipoEnimigo.SoldadoInterno:
                //    inimigo = new SoldadoInterno();
                //    break;
                //case (int)TipoEnimigo.SoldadoExterno:
                //    inimigo = new SoldadoExterno();
                default:
                    break;
            }
            return inimigo;
        }
        public int UpdateDefinicao(Heroi hero)
        {
            int count = 0;
            Vector2 posicaoToque;

            //Add an enemy at 100m and every 50 after
            //Should an additional enemy be added?
            if(hero.PosicaoX/20>adicionaDistanciaInimigo)
            {
                InimigoPatrulha inimigo = SpawnInimigos();
                listaInimigos.Add(inimigo);
                adicionaDistanciaInimigo += 50;
            }

            // destroy and respawn, update and collide with bubbles
            for(int i=listaInimigos.Count-1;i>=0;i--)
            {
                if(listaInimigos[i].DestroiFlag)
                {
                    listaInimigos.Remove(listaInimigos[i]);
                    listaInimigos.Add(SpawnInimigos());
                    continue;
                }
                //if(listaInimigos[i].UpdatePatrulha(hero, out posicaoToque))
                //{
                //    efeitoColisao.AdiconaAt(CriaParticulaVermelha, posicaoToque);
                //    count++;
                //}

                //List<Shooter> todosDiparos = hero.TodosDisparos();
                //int numDisparos = todosDiparos.Count;
                //for(int j=numDisparos-1;j>=0;j--)
                //{
                //    if(todosDiparos[j].ToquePixeis(listaInimigos[i], out posicaoToque))
                //    {
                //        listaInimigos[i].DefineParaEstadoStunned();
                //        todosDiparos.RemoveAt(j);
                //        //efeitoColisao.AddEmissor(criaParticulavermelha, posicaoToque);
                //    }

                //}
            }
            //efeitoColisao.UpdatePariculas();
            RespawnInimigos();
            return count;
        }
        //respawn fora da camera(esquerda)
        public void RespawnInimigos()
        {
            for (int i = listaInimigos.Count - 1; i >= 0; i--)
            {
                if (listaInimigos[i].PosicaoX < Camera.posicaoCantoInferiorEsq.X - listaInimigos[i].Largura)
                {
                    listaInimigos.Remove(listaInimigos[i]);
                    listaInimigos.Add(SpawnInimigos());
                }
            }
        }
        public void DrawSet()
        {
            foreach (var inimiigo in listaInimigos)
                inimiigo.Draw();
           // efeitoColissao.DrawSistemaParticulas();
        }
    }
}
