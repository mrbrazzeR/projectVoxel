using UnityEngine;
using UnityEngine.UI;

namespace GamePlay
{
    public class CameraRotate : MonoBehaviour
    {
        [SerializeField] private Button freeLook;
        [SerializeField] private GameObject freeLookObject;

        private bool _isActive=true;
        // Start is called before the first frame update
        void Start()
        {
            freeLook.onClick.AddListener(ActiveFreeLook);
        }

        // Update is called once per frame
        private void ActiveFreeLook()
        {
            freeLookObject.SetActive(_isActive);
            _isActive = !_isActive;
        }
    }
}
