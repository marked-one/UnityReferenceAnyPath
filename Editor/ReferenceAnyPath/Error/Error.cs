using System;
using System.Linq;

namespace ReferenceAnyPath {
    public abstract class Error<TValidator> where TValidator : Validator {
        protected Property Property { get; private set; }
        protected TValidator Validator { get; private set; }

        public void Init(Property property, TValidator validator) {
            Property = property;
            Validator = validator;
        }

        public bool IsError {
            get {
                if (Property.IsError)
                    return true;

                var absolutePath = Property.GetString(PropertyName._absolutePath).UnpackPathSimple();
                if (string.IsNullOrEmpty(absolutePath)) {
                    var path = Property.GetString(PropertyName._path).UnpackPathComplex();
                    return !string.IsNullOrEmpty(path);
                }

                var assetPath = Property.GetString(PropertyName._assetPath).UnpackPathSimple();

                return IsInvalidPath(absolutePath,assetPath);
            }
        }

        protected abstract bool IsInvalidPath(string absolutePath, string assetPath);
    }
}