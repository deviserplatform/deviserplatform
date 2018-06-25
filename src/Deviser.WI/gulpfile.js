/// <binding />

'use strict';

/*------------------------------------------------------------------
    Configuration section
-------------------------------------------------------------------*/
var paths = {
    webroot: './wwwroot/',
    sassPath: './wwwroot/css/admin/main.scss',
    skinSassPath: './wwwroot/css/site.scss',
    sassDest: './wwwroot/css/admin',
    skinSassDest: './wwwroot/css'
};



paths.bowerRoot = paths.webroot + 'lib/';
paths.appRoot = paths.webroot + 'js/src/app/';
paths.releaseRoot = paths.webroot + 'release/';

//Bower files selection
paths.bowerFiles = {
    js: [
        paths.bowerRoot + 'jquery/dist/jquery.js',
        paths.bowerRoot + 'jquery-ui/jquery-ui.js',
        paths.webroot + 'js/moment.js',
        paths.bowerRoot + 'bootstrap/dist/js/bootstrap.js',
        paths.bowerRoot + 'metisMenu/dist/metisMenu.js',
        paths.bowerRoot + 'lodash/dist/lodash.js',
        paths.bowerRoot + 'angular/angular.js',
        paths.webroot +'js/admin/main.js',
        paths.bowerRoot + 'angular-sanitize/angular-sanitize.js',
        paths.bowerRoot + 'angular-ui-router/release/angular-ui-router.js',
        paths.bowerRoot + 'angular-ui-sortable/sortable.js',
        paths.bowerRoot + 'angular-bootstrap/ui-bootstrap.js',
        paths.bowerRoot + 'angular-bootstrap/ui-bootstrap.js',
        paths.bowerRoot + 'angular-bootstrap/ui-bootstrap-tpls.js',
        paths.webroot + 'js/src/lib-fixes/select.js',
        paths.bowerRoot + 'angular-ui-tree/dist/angular-ui-tree.js',
        paths.bowerRoot + 'angular-drag-and-drop-lists/angular-drag-and-drop-lists.js',
        paths.bowerRoot + 'ng-file-upload/ng-file-upload-all.js',
        paths.bowerRoot + 'angular-img-cropper/dist/angular-img-cropper.min.js',
        paths.bowerRoot + 'textAngular/dist/textAngular-rangy.min.js',
        paths.bowerRoot + 'textAngular/dist/textAngularSetup.js',
        paths.bowerRoot + 'textAngular/dist/textAngular-sanitize.js',
        paths.bowerRoot + 'textAngular/dist/textAngular.js'
    ],
    css: [
        paths.bowerRoot + 'angular-ui-tree/dist/angular-ui-tree.min.css',
        paths.bowerRoot + 'angular-ui-select/dist/select.css',
        paths.bowerRoot + 'metismenu/dist/metismenu.css',
        paths.bowerRoot + 'textAngular/dist/textAngular.css',
    ],
    assets: [
        paths.bowerRoot + 'bootstrap/dist/fonts/**'
    ]
}

//App files selection
paths.appFiles = {
    js: [paths.appRoot + '**/*.js'],    
    assets: [paths.appRoot + 'assets/**/*.*']
}

//Minify file configuration
paths.concatLibJsDest = paths.releaseRoot + 'deviserlib.min.js';
paths.concatAppJsDest = paths.releaseRoot + 'deviserapp.min.js';
paths.concatCssDest = paths.releaseRoot + 'deviser.min.css';

/*-------------------------------------------------------------------
    Module Import
/*------------------------------------------------------------------*/
var es = require('event-stream'),
    del = require('del'),
    filelog = require('gulp-filelog'),
    gulp = require('gulp'),
    concat = require('gulp-concat'),
    cssmin = require('gulp-cssmin'),
    sass = require('gulp-sass'),
    uglify = require('gulp-uglify');


gulp.task("clean:sass", function () {
    return del(paths.sassDest + '/main.css', { force: true });
});

gulp.task('sass', function () {
    return gulp.src(paths.sassPath)
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest(paths.sassDest));
});

gulp.task('skin:sass', function () {
    return gulp.src(paths.skinSassPath)
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest(paths.skinSassDest));
});

gulp.task('clean:min', function () {    
    return del(paths.releaseRoot, { force: true });
});

gulp.task('clean', ['clean:min']);


gulp.task('min:libjs', function () {
    var scripts = paths.bowerFiles.js;
    return gulp.src(paths.bowerFiles.js)
        .pipe(concat(paths.concatLibJsDest))
        .pipe(uglify())
        .pipe(gulp.dest('.'));
});

gulp.task('min:appjs', function () {
    var scripts = paths.appFiles.js;
    //var templates = paths.tempFolder + '**/*.js';
    //scripts.push(templates);
    return gulp.src(scripts, { base: paths.appRoot })
        //.pipe(angularFilesort())
        .pipe(concat(paths.concatAppJsDest))
        .pipe(uglify())
        .pipe(gulp.dest('.'));
});

gulp.task('min:css', function () {

    var vendorFiles = gulp.src(paths.bowerFiles.css);
    var appFiles = gulp.src(paths.sassPath)
        .pipe(sass().on('error', sass.logError));

    var css = paths.bowerFiles.css;
    css = css.concat(paths.sassPath);
    return es.concat(vendorFiles, appFiles)
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest('.'));
});

gulp.task('test',
    function () {
        var css = paths.bowerFiles.css;
        css.push(paths.sassPath);

        return gulp.src(paths.sassPath)
            .pipe(filelog());

});

gulp.task('clean', ['clean:min']);

gulp.task('min', ['min:libjs', 'min:appjs', 'min:css']);

gulp.task('watch', ['sass'], function () {
    //var sassPaths = [];
    //sassPaths.push(paths.sassPath);
    //sassPaths.push(paths.skinSassPath);
    gulp.watch(paths.sassPath, ['sass']);
    gulp.watch(paths.skinSassPath, ['skin:sass']);
});


