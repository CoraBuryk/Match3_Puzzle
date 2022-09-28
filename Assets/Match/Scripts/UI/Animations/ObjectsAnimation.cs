using Assets.Match.Scripts.Audio;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Match.Scripts.UI.Animations
{
    public class ObjectsAnimation : MonoBehaviour
    {
        [SerializeField] private AudioEffectsGame _audioEffectsGame;

        public async void BombAnimation(GameObject bomb, Vector2 finalPos)
        {
            Vector2[] path = new[] { new Vector2(0, 0), finalPos };
            await Task.Delay(800);
            _audioEffectsGame.PlayBonusSound();
            bomb.GetComponent<Rigidbody2D>().DOLocalPath(path, 1.5f, PathType.CatmullRom);
            bomb.GetComponent<RectTransform>().DOScale(new Vector2(0.6f, 0.6f), 1.5f);
        }

        public async void RocketAnimation(GameObject rocket, Vector2 finalPos)
        {
            Vector2[] path = new[] { new Vector2(0, 0), finalPos };
            await Task.Delay(800);
            _audioEffectsGame.PlayBonusSound();
            rocket.GetComponent<Rigidbody2D>().DOLocalPath(path, 1.5f, PathType.CatmullRom);
            rocket.GetComponent<RectTransform>().DOScale(new Vector2(0.55f, 0.55f), 1.5f);
        }

        public async void Stars(GameObject star, float x, float y , int delay)
        {
            star.transform.localScale = new Vector3(4f, 4f, 0);
            await Task.Delay(delay);
            Vector2[] path = new[] { new Vector2(0, 0), new Vector2(x, y) };
            star.GetComponent<Rigidbody2D>().DOLocalPath(path, 0.5f, PathType.CatmullRom);
            star.GetComponent<RectTransform>().DOScale(new Vector2(2f, 2f), 0.5f);
        }
    }
}
