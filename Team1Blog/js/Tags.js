
(function () {
    var app = angular.module("myapp", []);
    app.controller("tags", function ($scope, $http) {
        try {


            var s = [];
            $scope.test = "angular test";
            $scope.addTag = function () {


                var r = $scope.event;
                if (r.search(",") > 0) {

                    s.push($scope.event);

                    $scope.event = null;
                    $scope.tags = s;
                    $scope.str = s.toString();
                }
            }
            $scope.remove = function ($index) {
                s.splice($index, 1);
                $scope.str = s.toString();
            }
        } catch (e) {
            alert(e.toString());
        }
    });


})();
