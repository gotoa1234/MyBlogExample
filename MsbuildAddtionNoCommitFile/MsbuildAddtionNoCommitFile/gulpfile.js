/// <binding AfterBuild='default' />
var gulp = require('gulp'),
    concat = require('gulp-concat'),
    uglify = require('gulp-uglify');

gulp.task('default',async function () {
    return gulp.src('Scripts/*.js')
        .pipe(uglify())
        .pipe(concat('all.js'))
        .pipe(gulp.dest('Scripts/MyJs'));
});