using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscapeFromWar.GraphicsSupport
{
    public class Shooting : ObjectoDeJogo
    {
        private const float BalaLargura = 50f;
        private const float BalaAltura = 50f;
        private const float BalaVelocidade = 1.8f;

        public Shooting(Vector2 posicao, int direcaoBala) : base("e", posicao, new Vector2(BalaLargura, BalaAltura), null)
        {
            Velocidade = BalaVelocidade;
            DirecaoVelocidade = new Vector2(direcaoBala, 0);
        }

        //Verifica se o sprite da bala tocou nos limites da janela
        public bool BalaEstaNoEcra()
        {
            Camera.EstadoColisaoCamera estado = Camera.ColisaoComJanela(this);
            return (Camera.EstadoColisaoCamera.DentroJanela == estado);
        }
    }
}
