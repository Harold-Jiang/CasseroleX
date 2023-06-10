require.config({
    urlArgs: "v=" + requirejs.s.contexts._.config.config.site.version,
    packages: [{
        name: 'moment',
        location: '../libs/moment',
        main: 'moment'
    }],
    //在打包压缩时将会把include中的模块合并到主文件中
    include: ['css', 'layer', 'toastr', 'fast', 'frontend', 'frontend-init', 'table', 'form', 'dragsort', 'drag', 'drop', 'selectpage'],
    paths: {
        'lang': "empty:",
        'form': 'require-form',
        'table': 'require-table',
        'upload': 'require-upload',
        'drag': 'jquery.drag.min',
        'drop': 'jquery.drop.min',
        'dropzone': 'dropzone.min',
        'echarts': 'echarts.min',
        'echarts-theme': 'echarts-theme',
        'adminlte': 'adminlte',
        'bootstrap-table-commonsearch': 'bootstrap-table-commonsearch',
        'bootstrap-table-template': 'bootstrap-table-template',
        // 以下的包从libs目录加载
        'jquery': '../libs/jquery/dist/jquery.min',
        'bootstrap': '../libs/bootstrap/dist/js/bootstrap.min',
        'bootstrap-datetimepicker': '../libs/eonasdan-bootstrap-datetimepicker/build/js/bootstrap-datetimepicker.min',
        'bootstrap-daterangepicker': '../libs/bootstrap-daterangepicker/daterangepicker',
        'bootstrap-select': '../libs/bootstrap-select/dist/js/bootstrap-select.min',
        'bootstrap-select-lang': '../libs/bootstrap-select/dist/js/i18n/defaults-zh_CN',
        'bootstrap-table': '../libs/fastadmin-bootstraptable/dist/bootstrap-table.min',
        'bootstrap-table-export': '../libs/fastadmin-bootstraptable/dist/extensions/export/bootstrap-table-export.min',
        'bootstrap-table-fixed-columns': '../libs/fastadmin-bootstraptable/dist/extensions/fixed-columns/bootstrap-table-fixed-columns',
        'bootstrap-table-mobile': '../libs/fastadmin-bootstraptable/dist/extensions/mobile/bootstrap-table-mobile.min',
        'bootstrap-table-lang': '../libs/fastadmin-bootstraptable/dist/locale/bootstrap-table-zh-CN',
        'bootstrap-table-jumpto': '../libs/fastadmin-bootstraptable/dist/extensions/page-jumpto/bootstrap-table-jumpto',
        'bootstrap-slider': '../libs/bootstrap-slider/dist/bootstrap-slider.min',
        'tableexport': '../libs/tableexport.jquery.plugin/tableExport.min',
        'dragsort': '../libs/fastadmin-dragsort/jquery.dragsort',
        'sortable': '../libs/sortablejs/Sortable.min',
        'addtabs': '../libs/fastadmin-addtabs/jquery.addtabs',
        'slimscroll': '../libs/jquery-slimscroll/jquery.slimscroll',
        'validator': '../libs/nice-validator/dist/jquery.validator',
        'validator-lang': '../libs/nice-validator/dist/local/zh-CN',
        'toastr': '../libs/toastr/build/toastr.min',
        'jstree': '../libs/jstree/dist/jstree.min',
        'layer': '../libs/fastadmin-layer/dist/layer',
        'cookie': '../libs/jquery.cookie/jquery.cookie',
        'cxselect': '../libs/fastadmin-cxselect/js/jquery.cxselect',
        'template': '../libs/art-template/dist/template-native',
        'selectpage': '../libs/fastadmin-selectpage/selectpage',
        'citypicker': '../libs/fastadmin-citypicker/dist/js/city-picker.min',
        'citypicker-data': '../libs/fastadmin-citypicker/dist/js/city-picker.data'
    },
    // shim依赖配置
    shim: {
        'addons': ['frontend'],
        'bootstrap': ['jquery'],
        'bootstrap-table': {
            deps: ['bootstrap'],
            exports: '$.fn.bootstrapTable'
        },
        'bootstrap-table-lang': {
            deps: ['bootstrap-table'],
            exports: '$.fn.bootstrapTable.defaults'
        },
        'bootstrap-table-export': {
            deps: ['bootstrap-table'],
            exports: '$.fn.bootstrapTable.defaults'
        },
        'bootstrap-table-fixed-columns': {
            deps: ['bootstrap-table'],
            exports: '$.fn.bootstrapTable.defaults'
        },
        'bootstrap-table-mobile': {
            deps: ['bootstrap-table'],
            exports: '$.fn.bootstrapTable.defaults'
        },
        'bootstrap-table-advancedsearch': {
            deps: ['bootstrap-table'],
            exports: '$.fn.bootstrapTable.defaults'
        },
        'bootstrap-table-commonsearch': {
            deps: ['bootstrap-table'],
            exports: '$.fn.bootstrapTable.defaults'
        },
        'bootstrap-table-template': {
            deps: ['bootstrap-table', 'template'],
            exports: '$.fn.bootstrapTable.defaults'
        },
        'bootstrap-table-jumpto': {
            deps: ['bootstrap-table'],
            exports: '$.fn.bootstrapTable.defaults'
        },
        'tableexport': {
            deps: ['jquery'],
            exports: '$.fn.extend'
        },
        'slimscroll': {
            deps: ['jquery'],
            exports: '$.fn.extend'
        },
        'adminlte': {
            deps: ['bootstrap', 'slimscroll'],
            exports: '$.AdminLTE'
        },
        'table': [
            'moment', 'moment/locale/zh-cn',
            'bootstrap-table',
            'bootstrap-table-lang', 'bootstrap-table-export', 'bootstrap-table-commonsearch', 'bootstrap-table-template',
            'bootstrap-table-jumpto', 'bootstrap-table-fixed-columns'
        ],
        'bootstrap-daterangepicker': ['moment', 'moment/locale/zh-cn'],
        'bootstrap-datetimepicker': ['moment', 'moment/locale/zh-cn'],
        'bootstrap-select-lang': ['bootstrap-select'],
        'selectpage': {
            deps: ['jquery'],
            exports: '$.fn.extend'
        },
        'layer': {
            deps: ['jquery']
        },
        'jstree': ['css!../libs/jstree/dist/themes/default/style.css'],
        'validator-lang': ['validator'],
        'citypicker': ['citypicker-data', 'css!../libs/fastadmin-citypicker/dist/css/city-picker.css']
    },
    baseUrl: requirejs.s.contexts._.config.config.site.cdnurl + '/assets/js/', //资源基础路径
    map: {
        '*': {
            'css': '../libs/require-css/css.min'
        }
    },
    waitSeconds: 60,
    charset: 'utf-8' // 文件编码
});

require(['jquery', 'bootstrap'], function ($, undefined) {
    //初始配置
    var Config = requirejs.s.contexts._.config.config;
    //将Config渲染到全局
    window.Config = Config;
    // 配置语言包的路径
    var paths = {};
    paths['lang'] = Config.moduleurl + '/ajax/lang?callback=define&controllername=' + Config.controllername + '&lang=' + Config.language + '&v=' + Config.site.version;
    // 避免目录冲突
    paths['frontend/'] = 'frontend/';
    // 如果是英文，则移除默认的定义
    if (Config.language === 'en') {
        $.each(requirejs.s.contexts._.config.paths, function (key, value) {
            if (key.match(/\-lang$/)) {
                define(key);
            }
        });
        define('moment/locale/zh-cn');
    }
    require.config({paths: paths});

    // 初始化
    $(function () {
        require(['fast'], function (Fast) {
            require(['frontend', 'frontend-init', 'addons'], function (Frontend, Addons) {
                //加载相应模块
                if (Config.jsname) {
                    require([Config.jsname], function (Controller) {
                        if (Controller.hasOwnProperty(Config.actionname)) {
                            Controller[Config.actionname]();
                        } else {
                            if (Controller.hasOwnProperty("_empty")) {
                                Controller._empty();
                            }
                        }
                    }, function (e) {
                        console.error(e);
                        // 这里可捕获模块加载的错误
                    });
                }
            });
        });
    });
});
