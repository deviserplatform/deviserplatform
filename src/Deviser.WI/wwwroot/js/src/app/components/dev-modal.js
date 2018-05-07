(function () {
    var app = angular.module('dev.modal', ['ui.bootstrap']);
    app.factory('modalService', ['$q', '$uibModal',modalService]);

    app.controller('ModalInstanceCtrl', ['$uibModalInstance', '$scope', 'labels', modalCtrl])


    function modalService($q, $uibModal) {
        var service = {};
        service.showConfirmation = showConfirmation;
        return service;


        function showConfirmation(labels) {
            var defer = $q.defer();
            var modalInstance = $uibModal.open({
                animation: true,
                backdrop: 'static',
                templateUrl: 'app/components/confirmdialog.tpl.html',
                controller: 'ModalInstanceCtrl as mVM',
                resolve: {
                    labels: function () {
                        return labels;
                    }
                }
            });

            return modalInstance.result;
        }
    }


    function modalCtrl($uibModalInstance, $scope, labels) {
        var vm = this;
        vm.labels = labels;

        vm.ok = ok;
        vm.cancel = cancel;

        function ok() {
            $uibModalInstance.close('ok');
        }

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        }
    }

}());