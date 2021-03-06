using System;
using System.Collections.Generic;
using System.Reflection;

namespace ILRuntime.Runtime.Generated
{
    class CLRBindings
    {

//will auto register in unity
#if UNITY_5_3_OR_NEWER
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.BeforeSceneLoad)]
#endif
        static private void RegisterBindingAction()
        {
            ILRuntime.Runtime.CLRBinding.CLRBindingUtils.RegisterBindingAction(Initialize);
        }


        /// <summary>
        /// Initialize the CLR binding, please invoke this AFTER CLR Redirection registration
        /// </summary>
        public static void Initialize(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            System_Threading_Interlocked_Binding.Register(app);
            System_Collections_Generic_List_1_ILTypeInstance_Binding.Register(app);
            EventHelper_Binding.Register(app);
            LuaFramework_BytesPack_Binding.Register(app);
            LuaFramework_NetworkManager_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Session_Binding.Register(app);
            LuaFramework_ByteBuffer_Binding.Register(app);
            System_String_Binding.Register(app);
            LuaFramework_Session_Binding.Register(app);
            UnityEngine_Time_Binding.Register(app);
            UnityEngine_GameObject_Binding.Register(app);
            UnityEngine_Object_Binding.Register(app);
            System_Collections_Generic_Queue_1_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Queue_1_String_Binding.Register(app);
            UnityEngine_MonoBehaviour_Binding.Register(app);
            UnityEngine_Transform_Binding.Register(app);
            UnityEngine_Component_Binding.Register(app);
            UnityEngine_UI_Text_Binding.Register(app);
            UnityEngine_UI_Graphic_Binding.Register(app);
            UnityEngine_UI_Button_Binding.Register(app);
            UnityEngine_Events_UnityEventBase_Binding.Register(app);
            UnityEngine_Vector3_Binding.Register(app);
            LuaFramework_Util_Binding.Register(app);
            UnityEngine_WaitForEndOfFrame_Binding.Register(app);
            UnityEngine_CanvasGroup_Binding.Register(app);
            UnityEngine_Vector2_Binding.Register(app);
            UnityEngine_RectTransform_Binding.Register(app);
            UnityEngine_WaitForSeconds_Binding.Register(app);
            DG_Tweening_DOTweenModuleUI_Binding.Register(app);
            DG_Tweening_ShortcutExtensions_Binding.Register(app);
            DG_Tweening_TweenSettingsExtensions_Binding.Register(app);
            System_NotSupportedException_Binding.Register(app);
            UnityEngine_SystemInfo_Binding.Register(app);
            UnityEngine_Application_Binding.Register(app);
            UnityEngine_Debug_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_ILTypeInstance_Binding.Register(app);
            System_Action_2_String_String_Binding.Register(app);
            System_Object_Binding.Register(app);
            TMPro_TMP_Text_Binding.Register(app);
            System_Collections_Generic_List_1_Transform_Binding.Register(app);
            System_Activator_Binding.Register(app);
            System_Type_Binding.Register(app);
            System_Reflection_MemberInfo_Binding.Register(app);
            LuaFramework_ILBehaviour_Binding.Register(app);
            System_Text_RegularExpressions_Regex_Binding.Register(app);
            System_Text_RegularExpressions_Group_Binding.Register(app);
            System_Text_RegularExpressions_Capture_Binding.Register(app);
            System_Collections_Generic_List_1_Int32_Binding.Register(app);
            System_Collections_Generic_List_1_Byte_Binding.Register(app);
            System_Int64_Binding.Register(app);
            System_Text_StringBuilder_Binding.Register(app);
            LuaFramework_MusicManager_Binding.Register(app);
            System_Boolean_Binding.Register(app);
            UnityEngine_PlayerPrefs_Binding.Register(app);
            AppFacade_Binding.Register(app);
            Facade_Binding.Register(app);
            DG_Tweening_DOTween_Binding.Register(app);
            System_Int32_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Int32_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Int32_Binding_KeyCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Boolean_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Boolean_Binding_KeyCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_ILTypeInstance_Binding_KeyCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Object_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Object_Binding_KeyCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Transform_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Transform_Binding_KeyCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Transform_Int32_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Transform_Int32_Binding_KeyCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Dictionary_2_Int32_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Dictionary_2_Int32_ILTypeInstance_Binding_KeyCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_List_1_Int32_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_List_1_Int32_Binding_KeyCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_List_1_ILTypeInstance_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_List_1_ILTypeInstance_Binding_KeyCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_List_1_Transform_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_List_1_Transform_Binding_KeyCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_String_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_String_Binding_KeyCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Boolean_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Boolean_Binding_KeyCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_ILTypeInstance_Binding_KeyCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Object_Binding_ValueCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Transform_Binding_ValueCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Transform_Int32_Binding_ValueCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_ILTypeInstance_Binding_ValueCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_ILTypeInstance_Binding_ValueCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Dictionary_2_Int32_ILTypeInstance_Binding_ValueCollection_Binding.Register(app);
            DG_Tweening_Tween_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Tween_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Tween_Binding_ValueCollection_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Int32_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Int32_Binding_ValueCollection_Binding.Register(app);
            System_Decimal_Binding.Register(app);
            System_Convert_Binding.Register(app);
            UnityEngine_UI_Image_Binding.Register(app);
            System_Single_Binding.Register(app);
            UnityEngine_Screen_Binding.Register(app);
            UnityEngine_UI_Toggle_Binding.Register(app);
            UnityEngine_Quaternion_Binding.Register(app);
            UnityEngine_UI_Selectable_Binding.Register(app);
            UnityEngine_Animator_Binding.Register(app);
            LitJson_JsonMapper_Binding.Register(app);
            UnityEngine_AudioSource_Binding.Register(app);
            UnityEngine_AudioClip_Binding.Register(app);
            System_Collections_Generic_List_1_String_Binding.Register(app);
            System_Collections_Generic_List_1_List_1_Int32_Binding.Register(app);
            System_Collections_Generic_List_1_Vector3_Binding.Register(app);
            UnityEngine_Input_Binding.Register(app);
            DragonBones_DragonBoneEventDispatcher_Binding.Register(app);
            DragonBones_UnityArmatureComponent_Binding.Register(app);
            DragonBones_Animation_Binding.Register(app);
            DragonBones_EventObject_Binding.Register(app);
            DragonBones_AnimationState_Binding.Register(app);
            UnityEngine_Color_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_String_Binding.Register(app);
            System_Collections_Generic_List_1_Vector2_Binding.Register(app);
            UnityEngine_UI_ScrollRect_Binding.Register(app);
            UnityEngine_Mathf_Binding.Register(app);
            UnityEngine_Random_Binding.Register(app);
            UnityEngine_Vector4_Binding.Register(app);
            DG_Tweening_TweenExtensions_Binding.Register(app);
            System_Collections_Generic_List_1_List_1_Transform_Binding.Register(app);
            System_Collections_Generic_List_1_List_1_Byte_Binding.Register(app);
            UnityEngine_Behaviour_Binding.Register(app);
            System_Collections_Generic_List_1_ScrollRect_Binding.Register(app);
            UnityEngine_Material_Binding.Register(app);
            Spine_Unity_SkeletonGraphic_Binding.Register(app);
            Spine_AnimationState_Binding.Register(app);
            UnityEngine_UI_Slider_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Single_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_Int32_Vector3_Binding.Register(app);
            System_Collections_Generic_List_1_Dictionary_2_String_Int32_Binding.Register(app);
            System_Collections_Generic_List_1_Int64_Binding.Register(app);
            System_Collections_Generic_List_1_Int16_Binding.Register(app);
            UnityEngine_Physics2D_Binding.Register(app);
            UnityEngine_QualitySettings_Binding.Register(app);
            UnityEngine_Resources_Binding.Register(app);
            UnityEngine_Camera_Binding.Register(app);
            System_GC_Binding.Register(app);
            System_Array_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Transform_Binding.Register(app);
            LuaFramework_EventTriggerHelper_Binding.Register(app);
            UnityEngine_EventSystems_PointerEventData_Binding.Register(app);
            System_Math_Binding.Register(app);
            LuaFramework_CollisionTriggerUtility_Binding.Register(app);
            System_Collections_Generic_Queue_1_Transform_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_Int64_Binding.Register(app);
            System_Collections_Generic_Stack_1_ILTypeInstance_Binding.Register(app);
            UnityEngine_LineRenderer_Binding.Register(app);
            System_Action_Binding.Register(app);
            System_Collections_Generic_Queue_1_Int64_Binding.Register(app);
            System_Collections_Generic_Queue_1_Single_Binding.Register(app);
            System_Collections_Generic_List_1_Single_Binding.Register(app);
            UnityEngine_ParticleSystem_Binding.Register(app);
            UnityEngine_Renderer_Binding.Register(app);
            UnityEngine_RaycastHit2D_Binding.Register(app);
            System_Collections_Generic_List_1_Material_Binding.Register(app);
            UnityEngine_AnimatorStateInfo_Binding.Register(app);
            System_Collections_Generic_Stack_1_Transform_Binding.Register(app);
            System_Collections_Generic_Dictionary_2_String_GameObject_Binding.Register(app);
            UnityEngine_LayerMask_Binding.Register(app);
            UnityEngine_Canvas_Binding.Register(app);
            DG_Tweening_ShortcutExtensionsTMPText_Binding.Register(app);
            UnityEngine_EventSystems_RaycastResult_Binding.Register(app);
            System_Collections_Generic_List_1_GameObject_Binding.Register(app);
            System_NotImplementedException_Binding.Register(app);
        }

        /// <summary>
        /// Release the CLR binding, please invoke this BEFORE ILRuntime Appdomain destroy
        /// </summary>
        public static void Shutdown(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
        }
    }
}
