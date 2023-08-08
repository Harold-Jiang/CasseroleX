define(['jquery', 'bootstrap', 'backend', 'table', 'form'], function ($, undefined, Backend, Table, Form) {

    var Controller = {
        index: function () {
            // Initialize Table Parameter Configuration
            Table.api.init({
                extend: {
                    index_url: 'auth/adminlog/index',
                    add_url: '',
                    edit_url: '',
                    del_url: 'auth/adminlog/del',
                    multi_url: 'auth/adminlog/multi',
                }
            });

            var table = $("#table");

            // Initialize Table
            table.bootstrapTable({
                url: $.fn.bootstrapTable.defaults.extend.index_url,
                fixedColumns: true,
                fixedRightNumber: 1,
                columns: [
                    [
                        {field: 'state', checkbox: true,},
                        {field: 'id', title: 'ID', operate: false},
                        {field: 'username', title: __('Username'), formatter: Table.api.formatter.search},
                        {field: 'title', title: __('Title'), operate: 'LIKE %...%', placeholder: '模糊搜索'},
                        {field: 'url', title: __('Url'), formatter: Table.api.formatter.url},
                        {field: 'ip', title: __('IP'), events: Table.api.events.ip, formatter: Table.api.formatter.search},
                        {field: 'browser', title: __('Browser'), operate: false, formatter: Controller.api.formatter.browser},
                        {field: 'createtime', title: __('Create time'), formatter: Table.api.formatter.datetime, operate: 'RANGE', addclass: 'datetimerange', sortable: true},
                        {
                            field: 'operate', title: __('Operate'), table: table,
                            events: Table.api.events.operate,
                            buttons: [{
                                name: 'detail',
                                text: __('Detail'),
                                icon: 'fa fa-list',
                                classname: 'btn btn-info btn-xs btn-detail btn-dialog',
                                url: 'auth/adminlog/detail'
                            }],
                            formatter: Table.api.formatter.operate
                        }
                    ]
                ]
            });

            // Bind events for tables
            Table.api.bindevent(table);
        },
        api: {
            bindevent: function () {
                Form.api.bindevent($("form[role=form]"));
            },
            formatter: {
                browser: function (value, row, index) {
                    return '<a class="btn btn-xs btn-browser">' + row.useragent.split(" ")[0] + '</a>';
                },
            },
        }
    };
    return Controller;
});
