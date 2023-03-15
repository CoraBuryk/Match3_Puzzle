using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using Assets.Match.Scripts.Models;

namespace Assets.Match.Scripts.UI.Animations
{

    public class ObstacleAnimation : MonoBehaviour
    {

        [SerializeField] private GameObject _crackedObstacle;

        public async void ObstaclesAnimation(Obstacles obstacle)
        {
            try
            {
                Vector3 pos = obstacle.transform.localPosition;
                GameObject cracked = Instantiate(_crackedObstacle, pos, Quaternion.identity);
                cracked.GetComponent<RectTransform>().DOLocalJump(new Vector2(pos.x, pos.y - 10), 10f, 1, 3f);
                Destroy(obstacle.GetComponent<Block>().gameObject);
                await Task.Delay(150);
                cracked.GetComponent<AudioSource>().Play();
                Destroy(cracked, 3f);
            }
            catch (System.Exception exception)
            {
                Debug.LogError(exception.Message);
                throw;
            }

        }
    }
}
