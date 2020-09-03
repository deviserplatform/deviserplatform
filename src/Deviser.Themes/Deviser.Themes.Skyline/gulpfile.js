const paths = {
    //webroot: './wwwroot/',
    //sassPath: './wwwroot/css/admin/main.scss',
    adminSassPath: './wwwroot/css/admin/admin.scss',
    skinSassPath: './wwwroot/css/site/site.scss',
    adminSassDest: './wwwroot/css/admin',
    skinSassDest: './wwwroot/css/site'
};

const gulp = require('gulp');
const sass = require('gulp-sass');
const del = require('del');

//gulp.task('style', () => {
//    return gulp.src('sass/**/*.scss')
//        .pipe(sass().on('error', sass.logError))
//        .pipe(gulp.dest('./css/'));
//});

//gulp.task('clean', () => {
//    return del([
//        'css/main.css',
//    ]);
//});

gulp.task("clean:skin", () => {
    return del(paths.skinSassDest + '/*.css', { force: true });
});

gulp.task("clean:admin", () => {
    return del(paths.adminSassDest + '/*.css', { force: true });
});

gulp.task('clean', gulp.parallel(['clean:skin', 'clean:admin']));

gulp.task('skin:sass', () => {
    return gulp.src(paths.skinSassPath)
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest(paths.skinSassDest));
});

gulp.task('admin:sass', () => {
    return gulp.src(paths.adminSassPath)
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest(paths.adminSassDest));
});

gulp.task('watch', ()=> {
    //var sassPaths = [];
    //sassPaths.push(paths.sassPath);
    //sassPaths.push(paths.skinSassPath);
    //gulp.watch(paths.sassPath, gulp.parallel('sass'));
    gulp.watch(paths.skinSassPath, gulp.parallel('skin:sass'));
    gulp.watch(paths.adminSassPath, gulp.parallel('admin:sass'));
});

gulp.task('default', gulp.series(['clean', 'skin:sass', 'admin:sass']));