(function () {
    var app = angular.module("myapp", []);
    app.controller("comment", function ($scope, $http) {
        try {


            var s = [];
            var r = "";
            $scope.addChar = function () {



                if ($scope.text.length < 513) {

                    r = $scope.text;
                    $scope.limit = false;
                    $scope.test = $scope.text.length;
                }


                else {
                    $scope.msg = "you reached to max allowed letters"
                    $scope.limit = true;
                    $scope.text = r;
                }

            }

        } catch (e) {
            alert(e.toString());
        }
    });
    app.controller("tags", function ($scope, $http) {
        try {


            var s = [];
            $scope.test = "angular test";
            $scope.addTag = function () {


                var r = $scope.event;
                if (r.search(",") > 0) {

                    s.push($scope.event);

                    $scope.event = null;
                    $scope.tages = s;
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
    app.controller("editetags", function ($scope, $http) {
        try {


            var s = [];
            $scope.test = [];
            var r=[];
            

            $scope.func = function () {
                $scope.test1 = $scope.test;
            }
            
            $scope.addTag = function () {


                var r = $scope.event;
                if (r.search(",") > 0) {

                    s.push($scope.event);

                    $scope.event = null;
                    $scope.tages = s;
                    $scope.str = s.toString();
                }
            }
            $scope.remove = function ($index) {
                s.splice($index, 1);
                $scope.str = s.toString();
            }

            $scope.delete = function ($index) { $scope.test1.pop($index, 1); }
           
            
        } catch (e) {
            alert(e.toString());
        }
    });
})();