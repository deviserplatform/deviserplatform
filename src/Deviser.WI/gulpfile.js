/// <binding Clean='clean' />
"use strict";

var del = require('del'),
    gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    sass = require('gulp-sass'),
    uglify = require("gulp-uglify");

var webroot = "./wwwroot/";

var paths = {
    js: webroot + "js/**/*.js",
    minJs: webroot + "js/**/*.min.js",
    css: webroot + "css/**/*.css",
    minCss: webroot + "css/**/*.min.css",
    concatJsDest: webroot + "js/site.min.js",
    concatCssDest: webroot + "css/site.min.css",
    sassPath: webroot + "css/admin/**/*.scss",
    sassDest: webroot + "css/admin"
};

gulp.task("clean:js", function () {    
    return del(paths.concatJsDest, { force: true });
});

gulp.task("clean:css", function () {
    return del(paths.concatCssDest, { force: true });
});

gulp.task("clean:sass", function () {    
    return del(paths.sassDest +'/main.css', { force: true });
});

gulp.task('sass', function () {
    return gulp.src(paths.sassPath)
      .pipe(sass().on('error', sass.logError))
      .pipe(gulp.dest(paths.sassDest));
});

gulp.task("min:js", function () {
    return gulp.src([paths.js, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("min:css", function () {
    return gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task('watch', ['sass'], function () {
    gulp.watch(paths.sassPath, ['sass']);
});

gulp.task("clean", ["clean:js", "clean:css", "clean:sass"]);
gulp.task("min", ["min:js", "min:css"]);
