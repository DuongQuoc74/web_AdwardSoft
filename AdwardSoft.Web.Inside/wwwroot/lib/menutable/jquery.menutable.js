(function ($, window, document, undefined) {
    var hasTouch = 'ontouchstart' in document;
    var divTitle = "";
    var hasPointerEvents = (function () {
        var el = document.createElement('div'),
            docEl = document.documentElement;
        if (!('pointerEvents' in el.style)) {
            return false;
        }
        el.style.pointerEvents = 'auto';
        el.style.pointerEvents = 'x';
        docEl.appendChild(el);
        var supports = window.getComputedStyle && window.getComputedStyle(el, '').pointerEvents === 'auto';
        docEl.removeChild(el);
        return !!supports;
    })();

    var defaults = {
        listNodeName: 'ol',
        itemNodeName: 'li',
        rootClass: 'mn',
        listClass: 'mn-list',
        itemClass: 'mn-item',
        dragClass: 'mn-dragel',
        handleClass: 'mn-handle',
        collapsedClass: 'mn-collapsed',
        placeClass: 'mn-placeholder',
        noDragClass: 'mn-nodrag',
        emptyClass: 'mn-empty',
        expandBtnHTML: '<button data-action="hidden" type="button"></button>',
        group: 0,
        maxDepth: 5,
        threshold: 20
    };

    function Plugin(element, options) {
        this.w = $(document);
        this.el = $(element);
        this.options = $.extend({}, defaults, options);
        this.init();
    }

    Plugin.prototype = {

        init: function () {
            var list = this;

            list.reset();

            list.el.data('menutable-group', this.options.group);

            list.placeEl = $('<div class="' + list.options.placeClass + '"/>');

            $.each(this.el.find(list.options.itemNodeName), function (k, el) {
                list.setParent($(el));
            });

            list.el.on('click', 'button', function (e) {
                if (list.dragEl) {
                    return;
                }
                var target = $(e.currentTarget),
                    action = target.data('action'),
                    item = target.parent(list.options.itemNodeName);
                if (action === 'hidden') {
                    list.collapseItem(item);
                }
                if (action === 'show') {
                    list.expandItem(item);
                }
                if (action === 'detail') {
                    list.detailItem(target.parent().data("id"), 'detail');
                }
                if (action === 'detail-base') {
                    list.detailItem(target.parent().data("id"), 'detail-base');
                }
                if (action === 'detail-lang') {
                    list.detailLangItem(target.parent().data("id"));
                }

                if (action === 'removed') {
                    list.removeItem(item);
                }
            });

            var onStartEvent = function (e) {
                var handle = $(e.target);
                if (!handle.hasClass(list.options.handleClass)) {
                    if (handle.closest('.' + list.options.noDragClass).length) {
                        return;
                    }
                    handle = handle.closest('.' + list.options.handleClass);
                }

                if (!handle.length || list.dragEl) {
                    return;
                }

                list.isTouch = /^touch/.test(e.type);
                if (list.isTouch && e.touches.length !== 1) {
                    return;
                }

                e.preventDefault();
                list.dragStart(e.touches ? e.touches[0] : e);
            };

            var onMoveEvent = function (e) {
                if (list.dragEl) {
                    e.preventDefault();
                    list.dragMove(e.touches ? e.touches[0] : e);
                }
            };

            var onEndEvent = function (e) {
                if (list.dragEl) {
                    e.preventDefault();
                    list.dragStop(e.touches ? e.touches[0] : e);
                }
            };

            if (hasTouch) {
                list.el[0].addEventListener('touchstart', onStartEvent, false);
                window.addEventListener('touchmove', onMoveEvent, false);
                window.addEventListener('touchend', onEndEvent, false);
                window.addEventListener('touchcancel', onEndEvent, false);
            }

            list.el.on('mousedown', onStartEvent);
            list.w.on('mousemove', onMoveEvent);
            list.w.on('mouseup', onEndEvent);

        },

        serialize: function () {
            var data,
                depth = 0,
                list = this;
            step = function (level, depth) {
                var array = [],
                    items = level.children(list.options.itemNodeName);
                items.each(function () {
                    var li = $(this),
                        item = $.extend({}, li.data()),
                        sub = li.children(list.options.listNodeName);
                    if (sub.length) {
                        item.children = step(sub, depth + 1);
                    }
                    array.push(item);
                });
                return array;
            };
            data = step(list.el.find(list.options.listNodeName).first(), depth);
            return data;
        },

        serializeArray: function () {
            var array = [];
            var index = 1;
            var indexChild = 0;
            $(".mn-item").each(function () {
                var title = $(this).find("span").eq(0).text();
                var id = Number($(this).data("id"));
                var parentid = Number($(this).data("parentid"));
                var type = $(this).data("type");
                var url = $(this).data("url");
                var sort = 0
                if (Number(parentid) == Number(id)) {
                    sort = index;
                    parentid = index
                    indexChild = 0;
                }
                else {
                    elparent = array.filter(function (e) { return e.typename + "." + e.referenceid == type + "." + Number(parentid) })
                    sort = indexChild + 1;
                    parentid = elparent[0].id;
                    indexChild++;
                }


                array.push({
                    title: title,
                    id: index,
                    referenceid: id,
                    parentid: parentid,
                    typename: type,
                    url: url,
                    sort: sort
                })
                index++;
            })
            return array;
        },

        serialise: function () {
            return this.serialize();
        },

        reset: function () {
            this.mouse = {
                offsetX: 0,
                offsetY: 0,
                startX: 0,
                startY: 0,
                lastX: 0,
                lastY: 0,
                nowX: 0,
                nowY: 0,
                distX: 0,
                distY: 0,
                dirAx: 0,
                dirX: 0,
                dirY: 0,
                lastDirX: 0,
                lastDirY: 0,
                distAxX: 0,
                distAxY: 0
            };
            this.isTouch = false;
            this.moving = false;
            this.dragEl = null;
            this.dragRootEl = null;
            this.dragDepth = 0;
            this.hasNewRoot = false;
            this.pointEl = null;
        },

        expandItem: function (li) {
            li.removeClass(this.options.collapsedClass);
            li.html(li.html().replace('data-action="show"', 'data-action="hidden"'))
            li.children(this.options.listNodeName).show("slow");
        },

        collapseItem: function (li) {
            var lists = li.children(this.options.listNodeName);
            if (lists.length) {
                li.html(li.html().replace('data-action="hidden"', 'data-action="show"'))
                li.children(this.options.listNodeName).slideUp("200")
                li.find(".mn-handle-details").removeClass("show")
            }
        },

        detailItem: function (li, action) {
            var div = $(".mn-handle-details[data-id='handleCollapse-" + li + "']")
            var divTitle = $("li[data-id='" + li + "'] .mn-handle");
            
            //if (!div.hasClass("show")) {
                $.ajax({
                    url: $("#menutable").attr("form"),
                    method: "GET",
                    data: { "id": li },
                    success: function (data) {
                        div.empty();
                        div.html(data);
                        if (action === 'detail') {
                            div.prev().toggleClass("rotate-90")
                            if (div.hasClass("show")) {
                                div.removeClass("show");
                                div.one("transitionend", function () {
                                    div.empty();
                                });
                            }
                            else {
                                div.addClass("show");
                            }
                        }
                        else {
                            if (!div.hasClass("show")) {
                                div.prev().toggleClass("rotate-90")
                                div.addClass("show");
                            }                            
                        }
                        
                        divTitle.addClass("non-border-bottom")
                    }
                });
            //}
            //if (action === 'detail') div.prev().toggleClass("rotate-90")
            //if (div.hasClass("show") && action === 'detail') {
            //    div.removeClass("show");
            //    div.one("transitionend", function () {
            //        div.empty();
            //    });
            //}
        },

        detailLangItem: function (li) {
            var div = $(".mn-handle-details[data-id='handleCollapse-" + li + "']")
            var divTitle = $("li[data-id='" + li + "'] .mn-handle");
            //div.prev().toggleClass("rotate-90")
            $.ajax({
                url: $("#menutable").attr("form-lang"),
                method: "GET",
                data: { "id": li },
                success: function (data) {
                    div.empty();
                    div.html(data);
                    div.addClass("show");
                    divTitle.addClass("non-border-bottom")
                }
            });
        },

        removeItem: function (parentLi) {
            var parentid = Number($(parentLi).data("parentid"));
            var id = Number($(parentLi).data("id"));
            var type = $(parentLi).data("type");
            if (id != parentid) {
                var l = $(parentLi).parent().find("li[data-parentid='" + parentid + "']").length;
                if (l > 1) {
                    $(parentLi).remove();
                    $("label.mn-item-check[data-type='" + type + "'][data-id='" + id + "']").find("i").removeClass("mn-item-title-active");
                    $("label.mn-item-check[data-type='" + type + "'][data-id='" + id + "']").find("input").prop("checked", false);
                }
                else {
                    $("li[data-id='" + parentid + "']").find("button[data-action='hidden']").remove();
                    $(parentLi).parent().remove();
                    $("label.mn-item-check[data-type='" + type + "'][data-id='" + id + "']").parent().find("i").removeClass("mn-item-title-active")
                    $("label.mn-item-check[data-type='" + type + "'][data-id='" + id + "']").parent().find("input").prop("checked", false);
                }

            }
            else {
                $("label.mn-item-check[data-type='" + type + "'][data-id='" + id + "']").find("i").removeClass("mn-item-title-active");
                $("label.mn-item-check[data-type='" + type + "'][data-id='" + id + "']").find("input").prop("checked", false);
                $(parentLi).remove();
            }


        },

        expandAll: function () {
            var list = this;
            list.el.find(list.options.itemNodeName).each(function () {
                list.expandItem($(this));
            });
        },

        collapseAll: function () {
            var list = this;
            list.el.find(list.options.itemNodeName).each(function () {
                list.collapseItem($(this));
            });
        },

        setParent: function (li) {
            if (li.children(this.options.listNodeName).length) {
                li.prepend($(this.options.expandBtnHTML));
            }
        },

        unsetParent: function (li) {
            li.removeClass(this.options.collapsedClass);
            li.children('[data-action="show"]').remove();
            li.children('[data-action="hidden"]').remove();
            li.children(this.options.listNodeName).remove();
        },

        dragStart: function (e) {
            divTitle = $(e.target).parent()
            $(".mn-handle-details").addClass("hidden")
            var mouse = this.mouse,
                target = $(e.target),
                dragItem = target.closest(this.options.itemNodeName);

            this.placeEl.css('height', dragItem.height());

            mouse.offsetX = e.offsetX !== undefined ? e.offsetX : e.pageX - target.offset().left;
            mouse.offsetY = e.offsetY !== undefined ? e.offsetY : e.pageY - target.offset().top;
            mouse.startX = mouse.lastX = e.pageX;
            mouse.startY = mouse.lastY = e.pageY;

            this.dragRootEl = this.el;

            this.dragEl = $(document.createElement(this.options.listNodeName)).addClass(this.options.listClass + ' ' + this.options.dragClass);
            this.dragEl.css('width', dragItem.width());

            dragItem.after(this.placeEl);
            dragItem[0].parentNode.removeChild(dragItem[0]);
            dragItem.appendTo(this.dragEl);

            $(document.body).append(this.dragEl);
            this.dragEl.css({
                'left': e.pageX - mouse.offsetX,
                'top': e.pageY - mouse.offsetY
            });
            // total depth of dragging item
            var i, depth,
                items = this.dragEl.find(this.options.itemNodeName);
            for (i = 0; i < items.length; i++) {
                depth = $(items[i]).parents(this.options.listNodeName).length;
                if (depth > this.dragDepth) {
                    this.dragDepth = depth;
                }
            }
        },

        dragStop: function (e) {
            var check = true;
            var el = this.dragEl.children(this.options.itemNodeName).first();
            var type = divTitle.data("type");
            if (type !== undefined) {
                var eltitle = this.placeEl.parents("li").data("type")
                if (eltitle !== undefined)
                    check = type === eltitle ? true : false
                if (check) {
                    var parentid = this.placeEl.parents("li").data("id");
                    divTitle.data("parentid", parentid ? parentid : divTitle.data("id"))
                }
            }
            if (check) {
                el[0].parentNode.removeChild(el[0]);
                this.placeEl.replaceWith(el);

                this.dragEl.remove();
                this.el.trigger('change');
                if (this.hasNewRoot) {
                    this.dragRootEl.trigger('change');
                }
                this.reset();
            }
        },

        dragMove: function (e) {
            var list, parent, prev, next, depth,
                opt = this.options,
                mouse = this.mouse;

            this.dragEl.css({
                'left': e.pageX - mouse.offsetX,
                'top': e.pageY - mouse.offsetY
            });

            // mouse position last events
            mouse.lastX = mouse.nowX;
            mouse.lastY = mouse.nowY;
            // mouse position this events
            mouse.nowX = e.pageX;
            mouse.nowY = e.pageY;
            // distance mouse moved between events
            mouse.distX = mouse.nowX - mouse.lastX;
            mouse.distY = mouse.nowY - mouse.lastY;
            // direction mouse was moving
            mouse.lastDirX = mouse.dirX;
            mouse.lastDirY = mouse.dirY;
            // direction mouse is now moving (on both axis)
            mouse.dirX = mouse.distX === 0 ? 0 : mouse.distX > 0 ? 1 : -1;
            mouse.dirY = mouse.distY === 0 ? 0 : mouse.distY > 0 ? 1 : -1;
            // axis mouse is now moving on
            var newAx = Math.abs(mouse.distX) > Math.abs(mouse.distY) ? 1 : 0;

            // do nothing on first move
            if (!mouse.moving) {
                mouse.dirAx = newAx;
                mouse.moving = true;
                return;
            }

            // calc distance moved on this axis (and direction)
            if (mouse.dirAx !== newAx) {
                mouse.distAxX = 0;
                mouse.distAxY = 0;
            } else {
                mouse.distAxX += Math.abs(mouse.distX);
                if (mouse.dirX !== 0 && mouse.dirX !== mouse.lastDirX) {
                    mouse.distAxX = 0;
                }
                mouse.distAxY += Math.abs(mouse.distY);
                if (mouse.dirY !== 0 && mouse.dirY !== mouse.lastDirY) {
                    mouse.distAxY = 0;
                }
            }
            mouse.dirAx = newAx;

            /**
             * move horizontal
             */
            if (mouse.dirAx && mouse.distAxX >= opt.threshold) {
                // reset move distance on x-axis for new phase
                mouse.distAxX = 0;
                prev = this.placeEl.prev(opt.itemNodeName);
                // increase horizontal level if previous sibling exists and is not collapsed
                if (mouse.distX > 0 && prev.length && !prev.hasClass(opt.collapsedClass)) {
                    // cannot increase level when item above is collapsed
                    list = prev.find(opt.listNodeName).last();
                    // check if depth limit has reached
                    depth = this.placeEl.parents(opt.listNodeName).length;
                    if (depth + this.dragDepth <= opt.maxDepth) {
                        // create new sub-level if one doesn't exist
                        if (!list.length) {
                            list = $('<' + opt.listNodeName + '/>').addClass(opt.listClass);
                            list.append(this.placeEl);
                            prev.append(list);
                            this.setParent(prev);
                        } else {
                            // else append to next level up
                            list = prev.children(opt.listNodeName).last();
                            list.append(this.placeEl);
                        }
                    }
                }
                // decrease horizontal level
                if (mouse.distX < 0) {
                    // we can't decrease a level if an item preceeds the current one
                    next = this.placeEl.next(opt.itemNodeName);
                    if (!next.length) {
                        parent = this.placeEl.parent();
                        this.placeEl.closest(opt.itemNodeName).after(this.placeEl);
                        if (!parent.children().length) {
                            this.unsetParent(parent.parent());
                        }
                    }
                }
            }

            var isEmpty = false;

            // find list item under cursor
            if (!hasPointerEvents) {
                this.dragEl[0].style.visibility = 'himnen';
            }
            this.pointEl = $(document.elementFromPoint(e.pageX - document.body.scrollLeft, e.pageY - (window.pageYOffset || document.documentElement.scrollTop)));
            if (!hasPointerEvents) {
                this.dragEl[0].style.visibility = 'visible';
            }
            if (this.pointEl.hasClass(opt.handleClass)) {
                this.pointEl = this.pointEl.parent(opt.itemNodeName);
            }
            if (this.pointEl.hasClass(opt.emptyClass)) {
                isEmpty = true;
            }
            else if (!this.pointEl.length || !this.pointEl.hasClass(opt.itemClass)) {
                return;
            }

            // find parent list of item under cursor
            var pointElRoot = this.pointEl.closest('.' + opt.rootClass),
                isNewRoot = this.dragRootEl.data('menutable-id') !== pointElRoot.data('menutable-id');

            /**
             * move vertical
             */
            if (!mouse.dirAx || isNewRoot || isEmpty) {
                // check if groups match if dragging over new root
                if (isNewRoot && opt.group !== pointElRoot.data('menutable-group')) {
                    return;
                }
                // check depth limit
                depth = this.dragDepth - 1 + this.pointEl.parents(opt.listNodeName).length;
                if (depth > opt.maxDepth) {
                    return;
                }
                var before = e.pageY < (this.pointEl.offset().top + this.pointEl.height() / 2);
                parent = this.placeEl.parent();
                // if empty create new list to replace empty placeholder
                if (isEmpty) {
                    list = $(document.createElement(opt.listNodeName)).addClass(opt.listClass);
                    list.append(this.placeEl);
                    this.pointEl.replaceWith(list);
                }
                else if (before) {
                    this.pointEl.before(this.placeEl);
                }
                else {
                    this.pointEl.after(this.placeEl);
                }
                if (!parent.children().length) {
                    this.unsetParent(parent.parent());
                }
                if (!this.dragRootEl.find(opt.itemNodeName).length) {
                    this.dragRootEl.append('<div class="' + opt.emptyClass + '"/>');
                }
                // parent root list has changed
                if (isNewRoot) {
                    this.dragRootEl = pointElRoot;
                    this.hasNewRoot = this.el[0] !== this.dragRootEl[0];
                }
            }
        }
    };

    $.fn.menutable = function (params) {
        var lists = this,
            retval = this;

        lists.each(function () {
            var plugin = $(this).data("menutable");

            if (!plugin) {
                $(this).data("menutable", new Plugin(this, params));
                $(this).data("menutable-id", new Date().getTime());
            } else {
                if (typeof params === 'string' && typeof plugin[params] === 'function') {
                    retval = plugin[params]();
                }
            }
        });

        return retval || lists;
    };

})(window.jQuery || window.Zepto, window, document);

//treeChkMenu = function (params) {
//    var str = String("");
//    var div = "<div class='mn-chk'>";
//    var parentDiv = "<div class='mn-chk-tree mn-chk-scroll'>"
//    getDataChk(params.data);
//    var addnew = params.id == "CustomsDiv" ? "" : "<a class='mn-chk-select' id='selectAll-" + params.id + "'>Select All</a>"
//    parentDiv += div + str + "</div></div></div>";
//    var divfooter = "<div class='mn-chk-footer'>" + addnew
//        + "<a class='mn-chk-btn mn-chk-btn-right mn-chk-btn-light' id='Add-" + params.id + "'><i class='fa fa-plus mr-1'></i> Add to Menu</a></div>"
//    $("#" + params.id).append(parentDiv);
//    $("#" + params.id).append(divfooter);
//    DataChkLoad();
//    function getDataChk(data, currentNode = 0) {
//        var items = currentNode > 0 ? data.filter(function (e) { return e.parentId == currentNode && e.parentId != e.id }) : data.filter(function (e) { return e.id == e.parentId });
//        if (items.length > 0 && items != undefined) {
//            str += '<ol class="mn-chk-list">';
//            for (let i = 0; i < items.length; i++) {
//                str += '<li class="mn-chk-item">'
//                    + '<label class="mn-item-check"  data-id="' + items[i].id + '" data-type="' + params.id + '" data-parentid="' + items[i].parentId + '" >' + items[i].navigationLabel
//                    + '<input type="checkbox" data-id="' + items[i].id + '" data-type="' + params.id + '" data-parentid="' + items[i].parentId + '" data-url="' + items[i].url + '"/>'
//                    + '<span class="mn-item-title"><i class="fa fa-check"></i></span>'
//                    + '</label>'
//                getDataChk(data, items[i].id)
//                str += '</li>'
//            }
//            str += '</ol>';
//        }
//    }

//    $("#selectAll-" + params.id).click(function () {
//        var div = $("#" + params.id + " .mn-chk");
//        if ($(this).text().toLocaleLowerCase() == "select all") {
//            div.find("i").addClass("mn-item-title-active");
//            div.find("input").prop('checked', true)
//            $(this).text("Unselect All")
//        }
//        else {
//            div.find("i").removeClass("mn-item-title-active");
//            div.find("input").prop('checked', false)
//            $(this).text("Select All")
//        }
//    })

//    $("#Add-" + params.id).click(function () {
//        str = String("");
//        var arr = [];

//        $("#" + params.id + " .mn-chk-item input:checked").each(function () {
//            arr.push({
//                id: Number($(this).data("id")),
//                parentId: parseInt($(this).data("parentid")),
//                title: $(this).parent().text(),
//                type: $(this).data("type"),
//                url: $(this).data("url")
//            })
//        });
//        getDataMenu(arr);
//        ol = $("#menutable ol").length;
//        if (ol > 0) {
//            str = str.replace('<ol class="mn-list">', "");
//            $("#menutable ol").eq(0).append(str.substring(0, str.length - 5));
//        }
//        else {
//            $("#menutable").append(str);
//        }
//        $('#menutable').find("button[data-action='hidden']").remove();
//        $('#menutable').find("button[data-action='show']").remove();
//        $('#menutable').removeData();
//        $('#menutable').off();
//        $('#menutable').menutable();
//    })

//    function getDataMenu(data, currentNode = 0) {
//        var items = currentNode > 0 ? data.filter(function (e) { return e.parentId == currentNode && e.parentId != e.id }) : data.filter(function (e) { return e.id == e.parentId });
//        if (items.length > 0 && items != undefined) {
//            str += '<ol class="mn-list">';
//            for (let i = 0; i < items.length; i++) {
//                str += "<li class='mn-item' data-id='" + items[i].id + "' data-type='" + items[i].type + "' data-parentid='" + items[i].parentId + "' data-url='" + items[i].url + "'>"
//                    + "<div class='mn-handle'><span>" + items[i].title + "</span><span class='span-type'>" + items[i].type + "</span></div>"
//                    + "<button data-action='removed' type='button'></button>"
//                    + "<button data-action='detail' type='button'></button>"
//                getDataMenu(data, items[i].id)
//                str += '</li>'
//            }
//            str += '</ol>';
//        }
//    }

//    $("#" + params.id + " label.mn-item-check").click(function (e) {
//        if (e.target !== this)
//            return;
//        id = Number($(this).data("id"));
//        if ($(this).find("input").prop('checked')) {
//            $(this).find("i").removeClass("mn-item-title-active");
//            $(this).find("input").prop('checked', false)
//        }
//        else
//            CheckData(this, params.id);
//    }).children().click(function () {
//        return false;
//    });

//    function CheckData(element, type) {
//        $(element).find("i").addClass("mn-item-title-active");
//        $(element).find("input").prop('checked', true);
//        var idparent = Number($(element).data("parentid"));
//        var id = Number($(element).data("id"));
//        if (id !== idparent) {
//            CheckData(idparent, type);
//        }
//    }

//    function DataChkLoad() {
//        $(".mn-item").each(function () {
//            var id = $(this).data("reference")
//            var type = $(this).data("type");
//            if (id > 0) {
//                $("#" + id + "[data-type='" + type + "']").next().find("i").addClass("mn-item-title-active");
//                $("#" + id + "[data-type='" + type + "']").prop('checked', true);
//            }
//        })
//    }
//}