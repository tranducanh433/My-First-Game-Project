using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace GreenSlime.MenuPlugins
{
    public class CreateObjectWindow : EditorWindow
    {
        KindOfMovement kindOfMovement;
        Sprite objectImage;
        string objectName = "New Object";
        bool twoBoxCollider = true;

        [MenuItem("GameObject/Create Other/Create Game Object")]
        public static void ShowWindow()
        {
            GetWindow<CreateObjectWindow>("Create Object Window");
        }
        private void OnEnable()
        {
            objectImage = Resources.Load<Sprite>("DefaultObjectSprite");
        }
        private void OnGUI()
        {
            kindOfMovement = (KindOfMovement)EditorGUILayout.EnumPopup("Kind Of Game", kindOfMovement);
            objectName = EditorGUILayout.TextField("Object Name", objectName);
            objectImage = EditorGUILayout.ObjectField("Object Image", objectImage, typeof(Sprite), true) as Sprite;
            twoBoxCollider = EditorGUILayout.Toggle("Two Box Collider", twoBoxCollider);

            if (GUILayout.Button("Create"))
            {
                CreateCharacterGameObject();
                Close();
            }
        }



        public void CreateCharacterGameObject()
        {
            //Spawn New Object
            GameObject obj = new GameObject();

            if (objectName == "New Object")
                obj.name = objectImage.name;
            else
                obj.name = objectName;


            //Add Component
            SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
            sr.sprite = objectImage;
            obj.AddComponent<BoxCollider2D>();
            if (twoBoxCollider)
            {
                BoxCollider2D bc2d = obj.AddComponent<BoxCollider2D>();
                bc2d.isTrigger = true;
            }
            obj.AddComponent<Animator>();
            obj.AddComponent<AudioSource>();
            Rigidbody2D rb = obj.AddComponent<Rigidbody2D>();

            //Seting The Object
            rb.freezeRotation = true;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

            if(kindOfMovement == KindOfMovement.topDown)
            {
                rb.gravityScale = 0;
            }
        }
    }
}
#endif