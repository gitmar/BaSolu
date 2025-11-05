#define ICALL_TABLE_corlib 1

static int corlib_icall_indexes [] = {
256,
268,
269,
270,
271,
272,
273,
274,
275,
276,
279,
280,
281,
471,
472,
473,
503,
504,
505,
525,
526,
527,
528,
645,
646,
647,
650,
702,
703,
704,
707,
709,
711,
713,
718,
726,
727,
728,
729,
730,
731,
732,
733,
734,
735,
736,
737,
738,
739,
740,
741,
742,
744,
745,
746,
747,
748,
749,
750,
847,
848,
849,
850,
851,
852,
853,
854,
855,
856,
857,
858,
859,
860,
861,
862,
863,
865,
866,
867,
868,
869,
870,
871,
938,
939,
1009,
1016,
1019,
1021,
1026,
1027,
1029,
1030,
1034,
1036,
1037,
1039,
1041,
1042,
1045,
1046,
1047,
1050,
1052,
1055,
1057,
1059,
1068,
1138,
1140,
1142,
1152,
1153,
1154,
1155,
1157,
1164,
1165,
1166,
1167,
1168,
1176,
1177,
1178,
1182,
1183,
1185,
1189,
1190,
1191,
1475,
1676,
1677,
10046,
10047,
10049,
10050,
10051,
10052,
10053,
10054,
10056,
10058,
10060,
10061,
10062,
10073,
10075,
10082,
10084,
10086,
10088,
10139,
10145,
10146,
10148,
10149,
10150,
10151,
10152,
10154,
10156,
11431,
11435,
11437,
11438,
11439,
11440,
11707,
11708,
11709,
11710,
11730,
11731,
11732,
11734,
11736,
11791,
11886,
11888,
11890,
11900,
11901,
11902,
11903,
11904,
12386,
12387,
12392,
12393,
12433,
12471,
12478,
12485,
12496,
12500,
12526,
12550,
12607,
12613,
12615,
12627,
12629,
12630,
12631,
12638,
12654,
12674,
12675,
12683,
12685,
12692,
12693,
12696,
12698,
12703,
12709,
12710,
12717,
12719,
12731,
12734,
12735,
12736,
12747,
12756,
12762,
12763,
12764,
12766,
12767,
12784,
12786,
12800,
12823,
12824,
12825,
12850,
12855,
12885,
12886,
13556,
13570,
13666,
13667,
13890,
13891,
13899,
13900,
13901,
13907,
14005,
14722,
14723,
15480,
15482,
15483,
15488,
15498,
16980,
17001,
17003,
17005,
};
void ves_icall_System_Array_InternalCreate (int,int,int,int,int);
int ves_icall_System_Array_GetCorElementTypeOfElementTypeInternal (int);
int ves_icall_System_Array_IsValueOfElementTypeInternal (int,int);
int ves_icall_System_Array_CanChangePrimitive (int,int,int);
int ves_icall_System_Array_FastCopy (int,int,int,int,int);
int ves_icall_System_Array_GetLengthInternal_raw (int,int,int);
int ves_icall_System_Array_GetLowerBoundInternal_raw (int,int,int);
void ves_icall_System_Array_GetGenericValue_icall (int,int,int);
void ves_icall_System_Array_GetValueImpl_raw (int,int,int,int);
void ves_icall_System_Array_SetGenericValue_icall (int,int,int);
void ves_icall_System_Array_SetValueImpl_raw (int,int,int,int);
void ves_icall_System_Array_InitializeInternal_raw (int,int);
void ves_icall_System_Array_SetValueRelaxedImpl_raw (int,int,int,int);
void ves_icall_System_Runtime_RuntimeImports_ZeroMemory (int,int);
void ves_icall_System_Runtime_RuntimeImports_Memmove (int,int,int);
void ves_icall_System_Buffer_BulkMoveWithWriteBarrier (int,int,int,int);
int ves_icall_System_Delegate_AllocDelegateLike_internal_raw (int,int);
int ves_icall_System_Delegate_CreateDelegate_internal_raw (int,int,int,int,int);
int ves_icall_System_Delegate_GetVirtualMethod_internal_raw (int,int);
void ves_icall_System_Enum_GetEnumValuesAndNames_raw (int,int,int,int);
void ves_icall_System_Enum_InternalBoxEnum_raw (int,int,int64_t,int);
int ves_icall_System_Enum_InternalGetCorElementType (int);
void ves_icall_System_Enum_InternalGetUnderlyingType_raw (int,int,int);
int ves_icall_System_Environment_get_ProcessorCount ();
int ves_icall_System_Environment_get_TickCount ();
int64_t ves_icall_System_Environment_get_TickCount64 ();
void ves_icall_System_Environment_FailFast_raw (int,int,int,int);
int ves_icall_System_GC_GetCollectionCount (int);
void ves_icall_System_GC_register_ephemeron_array_raw (int,int);
int ves_icall_System_GC_get_ephemeron_tombstone_raw (int);
void ves_icall_System_GC_SuppressFinalize_raw (int,int);
void ves_icall_System_GC_ReRegisterForFinalize_raw (int,int);
void ves_icall_System_GC_GetGCMemoryInfo (int,int,int,int,int,int);
int ves_icall_System_GC_AllocPinnedArray_raw (int,int,int);
int ves_icall_System_Object_MemberwiseClone_raw (int,int);
double ves_icall_System_Math_Acos (double);
double ves_icall_System_Math_Acosh (double);
double ves_icall_System_Math_Asin (double);
double ves_icall_System_Math_Asinh (double);
double ves_icall_System_Math_Atan (double);
double ves_icall_System_Math_Atan2 (double,double);
double ves_icall_System_Math_Atanh (double);
double ves_icall_System_Math_Cbrt (double);
double ves_icall_System_Math_Ceiling (double);
double ves_icall_System_Math_Cos (double);
double ves_icall_System_Math_Cosh (double);
double ves_icall_System_Math_Exp (double);
double ves_icall_System_Math_Floor (double);
double ves_icall_System_Math_Log (double);
double ves_icall_System_Math_Log10 (double);
double ves_icall_System_Math_Pow (double,double);
double ves_icall_System_Math_Sin (double);
double ves_icall_System_Math_Sinh (double);
double ves_icall_System_Math_Sqrt (double);
double ves_icall_System_Math_Tan (double);
double ves_icall_System_Math_Tanh (double);
double ves_icall_System_Math_FusedMultiplyAdd (double,double,double);
double ves_icall_System_Math_Log2 (double);
double ves_icall_System_Math_ModF (double,int);
float ves_icall_System_MathF_Acos (float);
float ves_icall_System_MathF_Acosh (float);
float ves_icall_System_MathF_Asin (float);
float ves_icall_System_MathF_Asinh (float);
float ves_icall_System_MathF_Atan (float);
float ves_icall_System_MathF_Atan2 (float,float);
float ves_icall_System_MathF_Atanh (float);
float ves_icall_System_MathF_Cbrt (float);
float ves_icall_System_MathF_Ceiling (float);
float ves_icall_System_MathF_Cos (float);
float ves_icall_System_MathF_Cosh (float);
float ves_icall_System_MathF_Exp (float);
float ves_icall_System_MathF_Floor (float);
float ves_icall_System_MathF_Log (float);
float ves_icall_System_MathF_Log10 (float);
float ves_icall_System_MathF_Pow (float,float);
float ves_icall_System_MathF_Sin (float);
float ves_icall_System_MathF_Sinh (float);
float ves_icall_System_MathF_Sqrt (float);
float ves_icall_System_MathF_Tan (float);
float ves_icall_System_MathF_Tanh (float);
float ves_icall_System_MathF_FusedMultiplyAdd (float,float,float);
float ves_icall_System_MathF_Log2 (float);
float ves_icall_System_MathF_ModF (float,int);
void ves_icall_RuntimeMethodHandle_ReboxFromNullable_raw (int,int,int);
void ves_icall_RuntimeMethodHandle_ReboxToNullable_raw (int,int,int,int);
int ves_icall_RuntimeType_GetCorrespondingInflatedMethod_raw (int,int,int);
void ves_icall_RuntimeType_make_array_type_raw (int,int,int,int);
void ves_icall_RuntimeType_make_byref_type_raw (int,int,int);
void ves_icall_RuntimeType_make_pointer_type_raw (int,int,int);
void ves_icall_RuntimeType_MakeGenericType_raw (int,int,int,int);
int ves_icall_RuntimeType_GetMethodsByName_native_raw (int,int,int,int,int);
int ves_icall_RuntimeType_GetPropertiesByName_native_raw (int,int,int,int,int);
int ves_icall_RuntimeType_GetConstructors_native_raw (int,int,int);
void ves_icall_RuntimeType_GetInterfaceMapData_raw (int,int,int,int,int);
int ves_icall_System_RuntimeType_CreateInstanceInternal_raw (int,int);
void ves_icall_System_RuntimeType_AllocateValueType_raw (int,int,int,int);
void ves_icall_RuntimeType_GetDeclaringMethod_raw (int,int,int);
void ves_icall_System_RuntimeType_getFullName_raw (int,int,int,int,int);
void ves_icall_RuntimeType_GetGenericArgumentsInternal_raw (int,int,int,int);
int ves_icall_RuntimeType_GetGenericParameterPosition (int);
int ves_icall_RuntimeType_GetEvents_native_raw (int,int,int,int);
int ves_icall_RuntimeType_GetFields_native_raw (int,int,int,int,int);
void ves_icall_RuntimeType_GetInterfaces_raw (int,int,int);
int ves_icall_RuntimeType_GetNestedTypes_native_raw (int,int,int,int,int);
void ves_icall_RuntimeType_GetDeclaringType_raw (int,int,int);
void ves_icall_RuntimeType_GetName_raw (int,int,int);
void ves_icall_RuntimeType_GetNamespace_raw (int,int,int);
int ves_icall_RuntimeType_FunctionPointerReturnAndParameterTypes_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetAttributes (int);
int ves_icall_RuntimeTypeHandle_GetMetadataToken_raw (int,int);
void ves_icall_RuntimeTypeHandle_GetGenericTypeDefinition_impl_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_GetCorElementType (int);
int ves_icall_RuntimeTypeHandle_HasInstantiation (int);
int ves_icall_RuntimeTypeHandle_IsComObject_raw (int,int);
int ves_icall_RuntimeTypeHandle_IsInstanceOfType_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_HasReferences_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetArrayRank_raw (int,int);
void ves_icall_RuntimeTypeHandle_GetAssembly_raw (int,int,int);
void ves_icall_RuntimeTypeHandle_GetElementType_raw (int,int,int);
void ves_icall_RuntimeTypeHandle_GetModule_raw (int,int,int);
void ves_icall_RuntimeTypeHandle_GetBaseType_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_type_is_assignable_from_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_IsGenericTypeDefinition (int);
int ves_icall_RuntimeTypeHandle_GetGenericParameterInfo_raw (int,int);
int ves_icall_RuntimeTypeHandle_is_subclass_of_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_IsByRefLike_raw (int,int);
void ves_icall_System_RuntimeTypeHandle_internal_from_name_raw (int,int,int,int,int,int);
int ves_icall_System_String_FastAllocateString_raw (int,int);
int ves_icall_System_String_InternalIsInterned_raw (int,int);
int ves_icall_System_String_InternalIntern_raw (int,int);
int ves_icall_System_Type_internal_from_handle_raw (int,int);
int ves_icall_System_ValueType_InternalGetHashCode_raw (int,int,int);
int ves_icall_System_ValueType_Equals_raw (int,int,int,int);
int ves_icall_System_Threading_Interlocked_CompareExchange_Int (int,int,int);
void ves_icall_System_Threading_Interlocked_CompareExchange_Object (int,int,int,int);
int ves_icall_System_Threading_Interlocked_Decrement_Int (int);
int64_t ves_icall_System_Threading_Interlocked_Decrement_Long (int);
int ves_icall_System_Threading_Interlocked_Increment_Int (int);
int64_t ves_icall_System_Threading_Interlocked_Increment_Long (int);
int ves_icall_System_Threading_Interlocked_Exchange_Int (int,int);
void ves_icall_System_Threading_Interlocked_Exchange_Object (int,int,int);
int64_t ves_icall_System_Threading_Interlocked_CompareExchange_Long (int,int64_t,int64_t);
int64_t ves_icall_System_Threading_Interlocked_Exchange_Long (int,int64_t);
int64_t ves_icall_System_Threading_Interlocked_Read_Long (int);
int ves_icall_System_Threading_Interlocked_Add_Int (int,int);
int64_t ves_icall_System_Threading_Interlocked_Add_Long (int,int64_t);
void ves_icall_System_Threading_Monitor_Monitor_Enter_raw (int,int);
void mono_monitor_exit_icall_raw (int,int);
void ves_icall_System_Threading_Monitor_Monitor_pulse_raw (int,int);
void ves_icall_System_Threading_Monitor_Monitor_pulse_all_raw (int,int);
int ves_icall_System_Threading_Monitor_Monitor_wait_raw (int,int,int,int);
void ves_icall_System_Threading_Monitor_Monitor_try_enter_with_atomic_var_raw (int,int,int,int,int);
void ves_icall_System_Threading_Thread_StartInternal_raw (int,int,int);
void ves_icall_System_Threading_Thread_InitInternal_raw (int,int);
int ves_icall_System_Threading_Thread_GetCurrentThread ();
void ves_icall_System_Threading_InternalThread_Thread_free_internal_raw (int,int);
int ves_icall_System_Threading_Thread_GetState_raw (int,int);
void ves_icall_System_Threading_Thread_SetState_raw (int,int,int);
void ves_icall_System_Threading_Thread_ClrState_raw (int,int,int);
void ves_icall_System_Threading_Thread_SetName_icall_raw (int,int,int,int);
int ves_icall_System_Threading_Thread_YieldInternal ();
void ves_icall_System_Threading_Thread_SetPriority_raw (int,int,int);
void ves_icall_System_Runtime_Loader_AssemblyLoadContext_PrepareForAssemblyLoadContextRelease_raw (int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_GetLoadContextForAssembly_raw (int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFile_raw (int,int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalInitializeNativeALC_raw (int,int,int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFromStream_raw (int,int,int,int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalGetLoadedAssemblies_raw (int);
int ves_icall_System_GCHandle_InternalAlloc_raw (int,int,int);
void ves_icall_System_GCHandle_InternalFree_raw (int,int);
int ves_icall_System_GCHandle_InternalGet_raw (int,int);
void ves_icall_System_GCHandle_InternalSet_raw (int,int,int);
int ves_icall_System_Runtime_InteropServices_Marshal_GetLastPInvokeError ();
void ves_icall_System_Runtime_InteropServices_Marshal_SetLastPInvokeError (int);
void ves_icall_System_Runtime_InteropServices_Marshal_StructureToPtr_raw (int,int,int,int);
void ves_icall_System_Runtime_InteropServices_Marshal_PtrToStructureInternal_raw (int,int,int,int);
int ves_icall_System_Runtime_InteropServices_Marshal_SizeOfHelper_raw (int,int,int);
int ves_icall_System_Runtime_InteropServices_NativeLibrary_LoadByName_raw (int,int,int,int,int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InternalGetHashCode_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InternalTryGetHashCode_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetObjectValue_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetUninitializedObjectInternal_raw (int,int);
void ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InitializeArray_raw (int,int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetSpanDataFrom_raw (int,int,int,int);
void ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_RunClassConstructor_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_SufficientExecutionStack ();
int ves_icall_System_Reflection_Assembly_GetExecutingAssembly_raw (int,int);
int ves_icall_System_Reflection_Assembly_GetEntryAssembly_raw (int);
int ves_icall_System_Reflection_Assembly_InternalLoad_raw (int,int,int,int);
int ves_icall_System_Reflection_Assembly_InternalGetType_raw (int,int,int,int,int,int);
int ves_icall_System_Reflection_AssemblyName_GetNativeName (int);
int ves_icall_MonoCustomAttrs_GetCustomAttributesInternal_raw (int,int,int,int);
int ves_icall_MonoCustomAttrs_GetCustomAttributesDataInternal_raw (int,int);
int ves_icall_MonoCustomAttrs_IsDefinedInternal_raw (int,int,int);
int ves_icall_System_Reflection_FieldInfo_internal_from_handle_type_raw (int,int,int);
int ves_icall_System_Reflection_FieldInfo_get_marshal_info_raw (int,int);
int ves_icall_System_Reflection_LoaderAllocatorScout_Destroy (int);
int ves_icall_GetCurrentMethod_raw (int);
void ves_icall_System_Reflection_RuntimeAssembly_GetEntryPoint_raw (int,int,int);
void ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceNames_raw (int,int,int);
void ves_icall_System_Reflection_RuntimeAssembly_GetExportedTypes_raw (int,int,int);
void ves_icall_System_Reflection_RuntimeAssembly_GetInfo_raw (int,int,int,int);
int ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceInternal_raw (int,int,int,int,int);
void ves_icall_System_Reflection_Assembly_GetManifestModuleInternal_raw (int,int,int);
void ves_icall_System_Reflection_RuntimeAssembly_GetModulesInternal_raw (int,int,int);
void ves_icall_System_Reflection_RuntimeCustomAttributeData_ResolveArgumentsInternal_raw (int,int,int,int,int,int,int);
void ves_icall_RuntimeEventInfo_get_event_info_raw (int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_System_Reflection_EventInfo_internal_from_handle_type_raw (int,int,int);
int ves_icall_RuntimeFieldInfo_ResolveType_raw (int,int);
int ves_icall_RuntimeFieldInfo_GetParentType_raw (int,int,int);
int ves_icall_RuntimeFieldInfo_GetFieldOffset_raw (int,int);
int ves_icall_RuntimeFieldInfo_GetValueInternal_raw (int,int,int);
void ves_icall_RuntimeFieldInfo_SetValueInternal_raw (int,int,int,int);
int ves_icall_RuntimeFieldInfo_GetRawConstantValue_raw (int,int);
int ves_icall_reflection_get_token_raw (int,int);
void ves_icall_get_method_info_raw (int,int,int);
int ves_icall_get_method_attributes (int);
int ves_icall_System_Reflection_MonoMethodInfo_get_parameter_info_raw (int,int,int);
int ves_icall_System_MonoMethodInfo_get_retval_marshal_raw (int,int);
int ves_icall_System_Reflection_RuntimeMethodInfo_GetMethodFromHandleInternalType_native_raw (int,int,int,int);
int ves_icall_RuntimeMethodInfo_get_name_raw (int,int);
int ves_icall_RuntimeMethodInfo_get_base_method_raw (int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_InternalInvoke_raw (int,int,int,int,int);
void ves_icall_RuntimeMethodInfo_GetPInvoke_raw (int,int,int,int,int);
int ves_icall_RuntimeMethodInfo_MakeGenericMethod_impl_raw (int,int,int);
int ves_icall_RuntimeMethodInfo_GetGenericArguments_raw (int,int);
int ves_icall_RuntimeMethodInfo_GetGenericMethodDefinition_raw (int,int);
int ves_icall_RuntimeMethodInfo_get_IsGenericMethodDefinition_raw (int,int);
int ves_icall_RuntimeMethodInfo_get_IsGenericMethod_raw (int,int);
void ves_icall_InvokeClassConstructor_raw (int,int);
int ves_icall_InternalInvoke_raw (int,int,int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_System_Reflection_RuntimeModule_InternalGetTypes_raw (int,int);
void ves_icall_System_Reflection_RuntimeModule_GetGuidInternal_raw (int,int,int);
int ves_icall_System_Reflection_RuntimeModule_ResolveMethodToken_raw (int,int,int,int,int,int);
int ves_icall_RuntimeParameterInfo_GetTypeModifiers_raw (int,int,int,int,int,int);
void ves_icall_RuntimePropertyInfo_get_property_info_raw (int,int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_System_Reflection_RuntimePropertyInfo_internal_from_handle_type_raw (int,int,int);
int ves_icall_CustomAttributeBuilder_GetBlob_raw (int,int,int,int,int,int,int,int);
void ves_icall_DynamicMethod_create_dynamic_method_raw (int,int,int,int,int);
void ves_icall_AssemblyBuilder_basic_init_raw (int,int);
void ves_icall_AssemblyBuilder_UpdateNativeCustomAttributes_raw (int,int);
void ves_icall_ModuleBuilder_basic_init_raw (int,int);
void ves_icall_ModuleBuilder_set_wrappers_type_raw (int,int,int);
int ves_icall_ModuleBuilder_getUSIndex_raw (int,int,int);
int ves_icall_ModuleBuilder_getToken_raw (int,int,int,int);
int ves_icall_ModuleBuilder_getMethodToken_raw (int,int,int,int);
void ves_icall_ModuleBuilder_RegisterToken_raw (int,int,int,int);
int ves_icall_TypeBuilder_create_runtime_class_raw (int,int);
int ves_icall_System_IO_Stream_HasOverriddenBeginEndRead_raw (int,int);
int ves_icall_System_IO_Stream_HasOverriddenBeginEndWrite_raw (int,int);
int ves_icall_System_Diagnostics_Debugger_IsAttached_internal ();
int ves_icall_System_Diagnostics_Debugger_IsLogging ();
void ves_icall_System_Diagnostics_Debugger_Log (int,int,int);
int ves_icall_System_Diagnostics_StackFrame_GetFrameInfo (int,int,int,int,int,int,int,int);
void ves_icall_System_Diagnostics_StackTrace_GetTrace (int,int,int,int);
int ves_icall_Mono_RuntimeClassHandle_GetTypeFromClass (int);
void ves_icall_Mono_RuntimeGPtrArrayHandle_GPtrArrayFree (int);
int ves_icall_Mono_SafeStringMarshal_StringToUtf8 (int);
void ves_icall_Mono_SafeStringMarshal_GFree (int);
static void *corlib_icall_funcs [] = {
// token 256,
ves_icall_System_Array_InternalCreate,
// token 268,
ves_icall_System_Array_GetCorElementTypeOfElementTypeInternal,
// token 269,
ves_icall_System_Array_IsValueOfElementTypeInternal,
// token 270,
ves_icall_System_Array_CanChangePrimitive,
// token 271,
ves_icall_System_Array_FastCopy,
// token 272,
ves_icall_System_Array_GetLengthInternal_raw,
// token 273,
ves_icall_System_Array_GetLowerBoundInternal_raw,
// token 274,
ves_icall_System_Array_GetGenericValue_icall,
// token 275,
ves_icall_System_Array_GetValueImpl_raw,
// token 276,
ves_icall_System_Array_SetGenericValue_icall,
// token 279,
ves_icall_System_Array_SetValueImpl_raw,
// token 280,
ves_icall_System_Array_InitializeInternal_raw,
// token 281,
ves_icall_System_Array_SetValueRelaxedImpl_raw,
// token 471,
ves_icall_System_Runtime_RuntimeImports_ZeroMemory,
// token 472,
ves_icall_System_Runtime_RuntimeImports_Memmove,
// token 473,
ves_icall_System_Buffer_BulkMoveWithWriteBarrier,
// token 503,
ves_icall_System_Delegate_AllocDelegateLike_internal_raw,
// token 504,
ves_icall_System_Delegate_CreateDelegate_internal_raw,
// token 505,
ves_icall_System_Delegate_GetVirtualMethod_internal_raw,
// token 525,
ves_icall_System_Enum_GetEnumValuesAndNames_raw,
// token 526,
ves_icall_System_Enum_InternalBoxEnum_raw,
// token 527,
ves_icall_System_Enum_InternalGetCorElementType,
// token 528,
ves_icall_System_Enum_InternalGetUnderlyingType_raw,
// token 645,
ves_icall_System_Environment_get_ProcessorCount,
// token 646,
ves_icall_System_Environment_get_TickCount,
// token 647,
ves_icall_System_Environment_get_TickCount64,
// token 650,
ves_icall_System_Environment_FailFast_raw,
// token 702,
ves_icall_System_GC_GetCollectionCount,
// token 703,
ves_icall_System_GC_register_ephemeron_array_raw,
// token 704,
ves_icall_System_GC_get_ephemeron_tombstone_raw,
// token 707,
ves_icall_System_GC_SuppressFinalize_raw,
// token 709,
ves_icall_System_GC_ReRegisterForFinalize_raw,
// token 711,
ves_icall_System_GC_GetGCMemoryInfo,
// token 713,
ves_icall_System_GC_AllocPinnedArray_raw,
// token 718,
ves_icall_System_Object_MemberwiseClone_raw,
// token 726,
ves_icall_System_Math_Acos,
// token 727,
ves_icall_System_Math_Acosh,
// token 728,
ves_icall_System_Math_Asin,
// token 729,
ves_icall_System_Math_Asinh,
// token 730,
ves_icall_System_Math_Atan,
// token 731,
ves_icall_System_Math_Atan2,
// token 732,
ves_icall_System_Math_Atanh,
// token 733,
ves_icall_System_Math_Cbrt,
// token 734,
ves_icall_System_Math_Ceiling,
// token 735,
ves_icall_System_Math_Cos,
// token 736,
ves_icall_System_Math_Cosh,
// token 737,
ves_icall_System_Math_Exp,
// token 738,
ves_icall_System_Math_Floor,
// token 739,
ves_icall_System_Math_Log,
// token 740,
ves_icall_System_Math_Log10,
// token 741,
ves_icall_System_Math_Pow,
// token 742,
ves_icall_System_Math_Sin,
// token 744,
ves_icall_System_Math_Sinh,
// token 745,
ves_icall_System_Math_Sqrt,
// token 746,
ves_icall_System_Math_Tan,
// token 747,
ves_icall_System_Math_Tanh,
// token 748,
ves_icall_System_Math_FusedMultiplyAdd,
// token 749,
ves_icall_System_Math_Log2,
// token 750,
ves_icall_System_Math_ModF,
// token 847,
ves_icall_System_MathF_Acos,
// token 848,
ves_icall_System_MathF_Acosh,
// token 849,
ves_icall_System_MathF_Asin,
// token 850,
ves_icall_System_MathF_Asinh,
// token 851,
ves_icall_System_MathF_Atan,
// token 852,
ves_icall_System_MathF_Atan2,
// token 853,
ves_icall_System_MathF_Atanh,
// token 854,
ves_icall_System_MathF_Cbrt,
// token 855,
ves_icall_System_MathF_Ceiling,
// token 856,
ves_icall_System_MathF_Cos,
// token 857,
ves_icall_System_MathF_Cosh,
// token 858,
ves_icall_System_MathF_Exp,
// token 859,
ves_icall_System_MathF_Floor,
// token 860,
ves_icall_System_MathF_Log,
// token 861,
ves_icall_System_MathF_Log10,
// token 862,
ves_icall_System_MathF_Pow,
// token 863,
ves_icall_System_MathF_Sin,
// token 865,
ves_icall_System_MathF_Sinh,
// token 866,
ves_icall_System_MathF_Sqrt,
// token 867,
ves_icall_System_MathF_Tan,
// token 868,
ves_icall_System_MathF_Tanh,
// token 869,
ves_icall_System_MathF_FusedMultiplyAdd,
// token 870,
ves_icall_System_MathF_Log2,
// token 871,
ves_icall_System_MathF_ModF,
// token 938,
ves_icall_RuntimeMethodHandle_ReboxFromNullable_raw,
// token 939,
ves_icall_RuntimeMethodHandle_ReboxToNullable_raw,
// token 1009,
ves_icall_RuntimeType_GetCorrespondingInflatedMethod_raw,
// token 1016,
ves_icall_RuntimeType_make_array_type_raw,
// token 1019,
ves_icall_RuntimeType_make_byref_type_raw,
// token 1021,
ves_icall_RuntimeType_make_pointer_type_raw,
// token 1026,
ves_icall_RuntimeType_MakeGenericType_raw,
// token 1027,
ves_icall_RuntimeType_GetMethodsByName_native_raw,
// token 1029,
ves_icall_RuntimeType_GetPropertiesByName_native_raw,
// token 1030,
ves_icall_RuntimeType_GetConstructors_native_raw,
// token 1034,
ves_icall_RuntimeType_GetInterfaceMapData_raw,
// token 1036,
ves_icall_System_RuntimeType_CreateInstanceInternal_raw,
// token 1037,
ves_icall_System_RuntimeType_AllocateValueType_raw,
// token 1039,
ves_icall_RuntimeType_GetDeclaringMethod_raw,
// token 1041,
ves_icall_System_RuntimeType_getFullName_raw,
// token 1042,
ves_icall_RuntimeType_GetGenericArgumentsInternal_raw,
// token 1045,
ves_icall_RuntimeType_GetGenericParameterPosition,
// token 1046,
ves_icall_RuntimeType_GetEvents_native_raw,
// token 1047,
ves_icall_RuntimeType_GetFields_native_raw,
// token 1050,
ves_icall_RuntimeType_GetInterfaces_raw,
// token 1052,
ves_icall_RuntimeType_GetNestedTypes_native_raw,
// token 1055,
ves_icall_RuntimeType_GetDeclaringType_raw,
// token 1057,
ves_icall_RuntimeType_GetName_raw,
// token 1059,
ves_icall_RuntimeType_GetNamespace_raw,
// token 1068,
ves_icall_RuntimeType_FunctionPointerReturnAndParameterTypes_raw,
// token 1138,
ves_icall_RuntimeTypeHandle_GetAttributes,
// token 1140,
ves_icall_RuntimeTypeHandle_GetMetadataToken_raw,
// token 1142,
ves_icall_RuntimeTypeHandle_GetGenericTypeDefinition_impl_raw,
// token 1152,
ves_icall_RuntimeTypeHandle_GetCorElementType,
// token 1153,
ves_icall_RuntimeTypeHandle_HasInstantiation,
// token 1154,
ves_icall_RuntimeTypeHandle_IsComObject_raw,
// token 1155,
ves_icall_RuntimeTypeHandle_IsInstanceOfType_raw,
// token 1157,
ves_icall_RuntimeTypeHandle_HasReferences_raw,
// token 1164,
ves_icall_RuntimeTypeHandle_GetArrayRank_raw,
// token 1165,
ves_icall_RuntimeTypeHandle_GetAssembly_raw,
// token 1166,
ves_icall_RuntimeTypeHandle_GetElementType_raw,
// token 1167,
ves_icall_RuntimeTypeHandle_GetModule_raw,
// token 1168,
ves_icall_RuntimeTypeHandle_GetBaseType_raw,
// token 1176,
ves_icall_RuntimeTypeHandle_type_is_assignable_from_raw,
// token 1177,
ves_icall_RuntimeTypeHandle_IsGenericTypeDefinition,
// token 1178,
ves_icall_RuntimeTypeHandle_GetGenericParameterInfo_raw,
// token 1182,
ves_icall_RuntimeTypeHandle_is_subclass_of_raw,
// token 1183,
ves_icall_RuntimeTypeHandle_IsByRefLike_raw,
// token 1185,
ves_icall_System_RuntimeTypeHandle_internal_from_name_raw,
// token 1189,
ves_icall_System_String_FastAllocateString_raw,
// token 1190,
ves_icall_System_String_InternalIsInterned_raw,
// token 1191,
ves_icall_System_String_InternalIntern_raw,
// token 1475,
ves_icall_System_Type_internal_from_handle_raw,
// token 1676,
ves_icall_System_ValueType_InternalGetHashCode_raw,
// token 1677,
ves_icall_System_ValueType_Equals_raw,
// token 10046,
ves_icall_System_Threading_Interlocked_CompareExchange_Int,
// token 10047,
ves_icall_System_Threading_Interlocked_CompareExchange_Object,
// token 10049,
ves_icall_System_Threading_Interlocked_Decrement_Int,
// token 10050,
ves_icall_System_Threading_Interlocked_Decrement_Long,
// token 10051,
ves_icall_System_Threading_Interlocked_Increment_Int,
// token 10052,
ves_icall_System_Threading_Interlocked_Increment_Long,
// token 10053,
ves_icall_System_Threading_Interlocked_Exchange_Int,
// token 10054,
ves_icall_System_Threading_Interlocked_Exchange_Object,
// token 10056,
ves_icall_System_Threading_Interlocked_CompareExchange_Long,
// token 10058,
ves_icall_System_Threading_Interlocked_Exchange_Long,
// token 10060,
ves_icall_System_Threading_Interlocked_Read_Long,
// token 10061,
ves_icall_System_Threading_Interlocked_Add_Int,
// token 10062,
ves_icall_System_Threading_Interlocked_Add_Long,
// token 10073,
ves_icall_System_Threading_Monitor_Monitor_Enter_raw,
// token 10075,
mono_monitor_exit_icall_raw,
// token 10082,
ves_icall_System_Threading_Monitor_Monitor_pulse_raw,
// token 10084,
ves_icall_System_Threading_Monitor_Monitor_pulse_all_raw,
// token 10086,
ves_icall_System_Threading_Monitor_Monitor_wait_raw,
// token 10088,
ves_icall_System_Threading_Monitor_Monitor_try_enter_with_atomic_var_raw,
// token 10139,
ves_icall_System_Threading_Thread_StartInternal_raw,
// token 10145,
ves_icall_System_Threading_Thread_InitInternal_raw,
// token 10146,
ves_icall_System_Threading_Thread_GetCurrentThread,
// token 10148,
ves_icall_System_Threading_InternalThread_Thread_free_internal_raw,
// token 10149,
ves_icall_System_Threading_Thread_GetState_raw,
// token 10150,
ves_icall_System_Threading_Thread_SetState_raw,
// token 10151,
ves_icall_System_Threading_Thread_ClrState_raw,
// token 10152,
ves_icall_System_Threading_Thread_SetName_icall_raw,
// token 10154,
ves_icall_System_Threading_Thread_YieldInternal,
// token 10156,
ves_icall_System_Threading_Thread_SetPriority_raw,
// token 11431,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_PrepareForAssemblyLoadContextRelease_raw,
// token 11435,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_GetLoadContextForAssembly_raw,
// token 11437,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFile_raw,
// token 11438,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalInitializeNativeALC_raw,
// token 11439,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFromStream_raw,
// token 11440,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalGetLoadedAssemblies_raw,
// token 11707,
ves_icall_System_GCHandle_InternalAlloc_raw,
// token 11708,
ves_icall_System_GCHandle_InternalFree_raw,
// token 11709,
ves_icall_System_GCHandle_InternalGet_raw,
// token 11710,
ves_icall_System_GCHandle_InternalSet_raw,
// token 11730,
ves_icall_System_Runtime_InteropServices_Marshal_GetLastPInvokeError,
// token 11731,
ves_icall_System_Runtime_InteropServices_Marshal_SetLastPInvokeError,
// token 11732,
ves_icall_System_Runtime_InteropServices_Marshal_StructureToPtr_raw,
// token 11734,
ves_icall_System_Runtime_InteropServices_Marshal_PtrToStructureInternal_raw,
// token 11736,
ves_icall_System_Runtime_InteropServices_Marshal_SizeOfHelper_raw,
// token 11791,
ves_icall_System_Runtime_InteropServices_NativeLibrary_LoadByName_raw,
// token 11886,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InternalGetHashCode_raw,
// token 11888,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InternalTryGetHashCode_raw,
// token 11890,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetObjectValue_raw,
// token 11900,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetUninitializedObjectInternal_raw,
// token 11901,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InitializeArray_raw,
// token 11902,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetSpanDataFrom_raw,
// token 11903,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_RunClassConstructor_raw,
// token 11904,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_SufficientExecutionStack,
// token 12386,
ves_icall_System_Reflection_Assembly_GetExecutingAssembly_raw,
// token 12387,
ves_icall_System_Reflection_Assembly_GetEntryAssembly_raw,
// token 12392,
ves_icall_System_Reflection_Assembly_InternalLoad_raw,
// token 12393,
ves_icall_System_Reflection_Assembly_InternalGetType_raw,
// token 12433,
ves_icall_System_Reflection_AssemblyName_GetNativeName,
// token 12471,
ves_icall_MonoCustomAttrs_GetCustomAttributesInternal_raw,
// token 12478,
ves_icall_MonoCustomAttrs_GetCustomAttributesDataInternal_raw,
// token 12485,
ves_icall_MonoCustomAttrs_IsDefinedInternal_raw,
// token 12496,
ves_icall_System_Reflection_FieldInfo_internal_from_handle_type_raw,
// token 12500,
ves_icall_System_Reflection_FieldInfo_get_marshal_info_raw,
// token 12526,
ves_icall_System_Reflection_LoaderAllocatorScout_Destroy,
// token 12550,
ves_icall_GetCurrentMethod_raw,
// token 12607,
ves_icall_System_Reflection_RuntimeAssembly_GetEntryPoint_raw,
// token 12613,
ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceNames_raw,
// token 12615,
ves_icall_System_Reflection_RuntimeAssembly_GetExportedTypes_raw,
// token 12627,
ves_icall_System_Reflection_RuntimeAssembly_GetInfo_raw,
// token 12629,
ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceInternal_raw,
// token 12630,
ves_icall_System_Reflection_Assembly_GetManifestModuleInternal_raw,
// token 12631,
ves_icall_System_Reflection_RuntimeAssembly_GetModulesInternal_raw,
// token 12638,
ves_icall_System_Reflection_RuntimeCustomAttributeData_ResolveArgumentsInternal_raw,
// token 12654,
ves_icall_RuntimeEventInfo_get_event_info_raw,
// token 12674,
ves_icall_reflection_get_token_raw,
// token 12675,
ves_icall_System_Reflection_EventInfo_internal_from_handle_type_raw,
// token 12683,
ves_icall_RuntimeFieldInfo_ResolveType_raw,
// token 12685,
ves_icall_RuntimeFieldInfo_GetParentType_raw,
// token 12692,
ves_icall_RuntimeFieldInfo_GetFieldOffset_raw,
// token 12693,
ves_icall_RuntimeFieldInfo_GetValueInternal_raw,
// token 12696,
ves_icall_RuntimeFieldInfo_SetValueInternal_raw,
// token 12698,
ves_icall_RuntimeFieldInfo_GetRawConstantValue_raw,
// token 12703,
ves_icall_reflection_get_token_raw,
// token 12709,
ves_icall_get_method_info_raw,
// token 12710,
ves_icall_get_method_attributes,
// token 12717,
ves_icall_System_Reflection_MonoMethodInfo_get_parameter_info_raw,
// token 12719,
ves_icall_System_MonoMethodInfo_get_retval_marshal_raw,
// token 12731,
ves_icall_System_Reflection_RuntimeMethodInfo_GetMethodFromHandleInternalType_native_raw,
// token 12734,
ves_icall_RuntimeMethodInfo_get_name_raw,
// token 12735,
ves_icall_RuntimeMethodInfo_get_base_method_raw,
// token 12736,
ves_icall_reflection_get_token_raw,
// token 12747,
ves_icall_InternalInvoke_raw,
// token 12756,
ves_icall_RuntimeMethodInfo_GetPInvoke_raw,
// token 12762,
ves_icall_RuntimeMethodInfo_MakeGenericMethod_impl_raw,
// token 12763,
ves_icall_RuntimeMethodInfo_GetGenericArguments_raw,
// token 12764,
ves_icall_RuntimeMethodInfo_GetGenericMethodDefinition_raw,
// token 12766,
ves_icall_RuntimeMethodInfo_get_IsGenericMethodDefinition_raw,
// token 12767,
ves_icall_RuntimeMethodInfo_get_IsGenericMethod_raw,
// token 12784,
ves_icall_InvokeClassConstructor_raw,
// token 12786,
ves_icall_InternalInvoke_raw,
// token 12800,
ves_icall_reflection_get_token_raw,
// token 12823,
ves_icall_System_Reflection_RuntimeModule_InternalGetTypes_raw,
// token 12824,
ves_icall_System_Reflection_RuntimeModule_GetGuidInternal_raw,
// token 12825,
ves_icall_System_Reflection_RuntimeModule_ResolveMethodToken_raw,
// token 12850,
ves_icall_RuntimeParameterInfo_GetTypeModifiers_raw,
// token 12855,
ves_icall_RuntimePropertyInfo_get_property_info_raw,
// token 12885,
ves_icall_reflection_get_token_raw,
// token 12886,
ves_icall_System_Reflection_RuntimePropertyInfo_internal_from_handle_type_raw,
// token 13556,
ves_icall_CustomAttributeBuilder_GetBlob_raw,
// token 13570,
ves_icall_DynamicMethod_create_dynamic_method_raw,
// token 13666,
ves_icall_AssemblyBuilder_basic_init_raw,
// token 13667,
ves_icall_AssemblyBuilder_UpdateNativeCustomAttributes_raw,
// token 13890,
ves_icall_ModuleBuilder_basic_init_raw,
// token 13891,
ves_icall_ModuleBuilder_set_wrappers_type_raw,
// token 13899,
ves_icall_ModuleBuilder_getUSIndex_raw,
// token 13900,
ves_icall_ModuleBuilder_getToken_raw,
// token 13901,
ves_icall_ModuleBuilder_getMethodToken_raw,
// token 13907,
ves_icall_ModuleBuilder_RegisterToken_raw,
// token 14005,
ves_icall_TypeBuilder_create_runtime_class_raw,
// token 14722,
ves_icall_System_IO_Stream_HasOverriddenBeginEndRead_raw,
// token 14723,
ves_icall_System_IO_Stream_HasOverriddenBeginEndWrite_raw,
// token 15480,
ves_icall_System_Diagnostics_Debugger_IsAttached_internal,
// token 15482,
ves_icall_System_Diagnostics_Debugger_IsLogging,
// token 15483,
ves_icall_System_Diagnostics_Debugger_Log,
// token 15488,
ves_icall_System_Diagnostics_StackFrame_GetFrameInfo,
// token 15498,
ves_icall_System_Diagnostics_StackTrace_GetTrace,
// token 16980,
ves_icall_Mono_RuntimeClassHandle_GetTypeFromClass,
// token 17001,
ves_icall_Mono_RuntimeGPtrArrayHandle_GPtrArrayFree,
// token 17003,
ves_icall_Mono_SafeStringMarshal_StringToUtf8,
// token 17005,
ves_icall_Mono_SafeStringMarshal_GFree,
};
static uint8_t corlib_icall_flags [] = {
0,
0,
0,
0,
0,
4,
4,
0,
4,
0,
4,
4,
4,
0,
0,
0,
4,
4,
4,
4,
4,
0,
4,
0,
0,
0,
4,
0,
4,
4,
4,
4,
0,
4,
4,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
0,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
4,
0,
0,
0,
0,
0,
0,
0,
0,
0,
};
