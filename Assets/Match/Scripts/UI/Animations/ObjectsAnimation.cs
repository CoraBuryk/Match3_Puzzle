using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using Assets.Match.Scripts.Audio;
using Assets.Match.Scripts.Enum;
using Assets.Match.Scripts.Models;

namespace Assets.Match.Scripts.UI.Animations
{

    public class ObjectsAnimation : MonoBehaviour
    {
        [SerializeField] private AudioEffectsGame _audioEffectsGame;

        public async void BonusAnimation(GameObject bonus, Vector2 finalPos)
        {
            try
            {
                Vector2[] path = new[] { new Vector2(0, 0), finalPos };
                await Task.Delay(800);

                bonus.GetComponent<Rigidbody2D>().DOLocalPath(path, 1f, PathType.CatmullRom);
                if (bonus.GetComponent<Bonus>().Type == BonusType.Rocket)
                {
                    bonus.GetComponent<RectTransform>().DOScale(new Vector2(0.55f, 0.55f), .8f);
                }
                else if (bonus.GetComponent<Bonus>().Type == BonusType.Bomb)
                {
                    bonus.GetComponent<RectTransform>().DOScale(new Vector2(0.6f, 0.6f), .8f);
                }

                _audioEffectsGame.PlayBonusSound();
            }
            catch (System.Exception exception)
            {
                Debug.LogError(exception.Message);
                throw;
            }                   
        }

        public async void Stars(GameObject star, float x, float y , int delay)
        {
            try
            {
                star.transform.localScale = new Vector3(4f, 4f, 0);
                await Task.Delay(delay);
                Vector2[] path = new[] { new Vector2(0, 0), new Vector2(x, y) };
                star.GetComponent<Rigidbody2D>().DOLocalPath(path, 0.5f, PathType.CatmullRom);
                star.GetComponent<RectTransform>().DOScale(new Vector2(2f, 2f), 0.5f);
            }
            catch (System.Exception exception)
            {
                Debug.LogError(exception.Message);
                throw;
            }           
        }

    }
}
