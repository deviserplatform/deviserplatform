_.mixin({
    findDeep: function (items, attrs) {

        function match(value) {
            for (var key in attrs) {
                if (!_.isUndefined(value)) {
                    if (attrs[key] !== value[key]) {
                        return false;
                    }
                }
            }

            return true;
        }

        function traverse(value) {
            var result;

            _.forEach(value, function (val) {
                if (match(val)) {
                    result = val;
                    return false;
                }

                if (_.isObject(val) || _.isArray(val)) {
                    result = traverse(val);
                }

                if (result) {
                    return false;
                }
            });

            return result;
        }

        return traverse(items);

    }
});