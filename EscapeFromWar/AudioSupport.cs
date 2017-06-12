using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;


namespace EscapeFromWar
{
   static public class AudioSupport
    {
        private static Dictionary<String, SoundEffect> efeitosAudio = new Dictionary<string, SoundEffect>();

        private static SoundEffectInstance backgoundSound = null;

        static public void PlaySom(String nomeSom)
        {
            SoundEffect som = EncontraClipSom(nomeSom);
            if (som != null)
                som.Play();
        }

        private static SoundEffect EncontraClipSom(String nomeSom)
        {
            SoundEffect som = null;
            if (efeitosAudio.ContainsKey(nomeSom))
                som = efeitosAudio[nomeSom];
            else
            {
                som = Game1.content.Load<SoundEffect>(nomeSom);
                if (som != null)
                    efeitosAudio.Add(nomeSom, som);
            }
            return som;
        }

        static public void Background(String musica, float nivle)
        {
            StopBackground();
            if((""!=musica)||(null!=musica))
            {
                nivle = MathHelper.Clamp(nivle, 0f, 1f);
                StartBackGround(musica, nivle);
            }
        }

        private static void StartBackGround(String musica, float nivle)
        {
            SoundEffect backGround = EncontraClipSom(musica);
            backgoundSound = backGround.CreateInstance();
            backgoundSound.IsLooped = true;
            backgoundSound.Volume = nivle;
            backgoundSound.Play();
        }

       

        private static void StopBackground()
        {
            if(backgoundSound!=null)
            {
                backgoundSound.Pause();
                backgoundSound.Stop();
                backgoundSound.Volume = 0f;

                backgoundSound.Dispose();
            }
            backgoundSound = null;
        }
    }
}
