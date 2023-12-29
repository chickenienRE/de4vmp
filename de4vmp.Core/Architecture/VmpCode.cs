namespace de4vmp.Core.Architecture;

public enum VmpCode {
    CilOrCode,
    CilAndCode,
    CilXorCode,
    CilNotCode,
    CilNegCode,
    CilShlCode,

    CilAddCode,
    CilAddOvfCode,
    CilAddOvfUnCode,
    CilSubCode,
    CilSubOvfCode,
    CilSubOvfUnCode,
    CilMulCode,
    CilMulOvfCode,
    CilMulOvfUnCode,

    VmilCmpSignedCode,
    VmilCmpUnsignedCode,

    CilDivSignedCode,
    CilDivUnsignedCode,
    CilRemSignedCode,
    CilRemUnsignedCode,
    CilShrSignedCode,
    CilShrUnsignedCode,

    VmilBrCode,

    CilLdstrCode,
    CilLdnullCode,

    CilLdcI4Code,
    CilLdcI8Code,
    CilLdcR4Code,
    CilLdcR8Code,

    VmilLdindI4Code,

    VmilInitVarCode,
    VmilLoadVarCode,
    VmilLoadVarACode,
    VmilStoreVarCode,

    CilPopCode,
    CilDupCode,
    CilSizeOfCode,
    CilCkfiniteCode,
    CilLocallocCode,

    CilConvSignedCode,
    CilConvUnsignedCode,

    VmilConvCode,
    VmilLoadTypeCode,

    VmilLoadFieldCode,
    VmilLoadFieldACode,
    VmilStaticCallCode,
    VmilInstanceCallCode,

    CilCallCode,
    CilCalliCode,
    CilCallvirtCode,

    CilBoxCode,
    CilUnboxCode,
    CilUnboxAnyCode,
    CilLdftnCode,
    CilLdvirtftnCode,
    CilIsinstCode,
    CilNewobjCode,
    CilLdtokenCode,
    CilInitobjCode,
    CilCastclassCode,
    CilStfldCode,
    CilStsfldCode,

    CilLdlenCode,
    CilNewarrCode,
    CilLdelemCode,
    VmilLdelemaCode,
    CilStelemCode,

    VmilLdindCode,
    VmilStindCode,

    VmilPushEhCode,
    VmilInitEhCode,

    CilLeaveCode,
    CilEndfilterCode,
    CilEndfinallyCode,
    CilThrowCode,
    CilRethrowCode
}