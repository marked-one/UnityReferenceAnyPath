#if UNITY_EDITOR
using System;
using UnityObject = UnityEngine.Object;

namespace ReferenceAnyPath {
    public static class RuntimeAssetHelper {
        public static void OnBeforeSerialize(
            UnityObject @object,
            ref string path,
            ref string relativePath,
            ref string absolutePath,
            ref string assetPath,
            ref string runtimePath,
            ref bool error,
            Func<string, string> getRuntimePath) {
            AssetHelper.OnBeforeSerialize(
                @object,
                ref path,
                ref relativePath,
                ref absolutePath,
                ref assetPath,
                ref runtimePath,
                ref error,
                getRuntimePath);

#if !REFERENCE_ANY_PATH_NO_VALIDATION_BEFORE_SERIALIZATION
            HandleRuntimePath(absolutePath, runtimePath, ref error);
#endif
        }

        public static void OnBeforeSerialize(
            UnityObject @object,
            int width,
            int height,
            int bits,
            ref string path,
            ref string relativePath,
            ref string absolutePath,
            ref string assetPath,
            ref string runtimePath,
            ref bool error,
            Func<string, string> getRuntimePath) {
            AssetHelper.OnBeforeSerialize(
                @object,
                ref path,
                ref relativePath,
                ref absolutePath,
                ref assetPath,
                ref runtimePath,
                ref error,
                GetRuntimePath);

            string GetRuntimePath(string unpackedAssetPath) {
                if (width <= 0 || height <= 0 || bits <= 0)
                    return null;

                return getRuntimePath.Invoke(unpackedAssetPath);
            }

#if !REFERENCE_ANY_PATH_NO_VALIDATION_BEFORE_SERIALIZATION
            HandleRuntimePath(absolutePath, runtimePath, ref error);
#endif

            var unpackedAbsolutePath = absolutePath.UnpackPathSimple();
            if (unpackedAbsolutePath != null && @object != null && (width <= 0 || height <= 0 || bits <= 0))
                error = true;
        }

#if !REFERENCE_ANY_PATH_NO_VALIDATION_BEFORE_SERIALIZATION
        static void HandleRuntimePath(string absolutePath, string runtimePath, ref bool error) {
            var unpackedAbsolutePath = absolutePath.UnpackPathSimple();
            var unpackedRuntimePath = runtimePath.UnpackPathComplex();
            if (unpackedAbsolutePath == null || unpackedRuntimePath != null)
                return;

            error = true;
        }
#endif
    }
}
#endif