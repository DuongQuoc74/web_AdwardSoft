///**
//* @version: 2.1.25
//* @author: Dan Grossman http://www.dangrossman.info/
//* @copyright: Copyright (c) 2012-2017 Dan Grossman. All rights reserved.
//* @license: Licensed under the MIT license. See http://www.opensource.org/licenses/mit-license.php
//* @website: http://www.daterangepicker.com/
//*/
//// Follow the UMD template https://github.com/umdjs/umd/blob/master/templates/returnExportsGlobal.js
//(function (root, factory) {
//    if (typeof define === 'function' && define.amd) {
//        // AMD. Make globaly available as well
//        define(['moment', 'jquery'], function (moment, jquery) {
//            if (!jquery.fn) jquery.fn = {}; // webpack server rendering
//            return (root.daterangepicker = factory(moment, jquery));
//        });
//    } else if (typeof module === 'object' && module.exports) {
//        // Node / Browserify
//        //isomorphic issue
//        var jQuery = (typeof window != 'undefined') ? window.jQuery : undefined;
//        if (!jQuery) {
//            jQuery = require('jquery');
//            if (!jQuery.fn) jQuery.fn = {};
//        }
//        var moment = (typeof window != 'undefined' && typeof window.moment != 'undefined') ? window.moment : require('moment');
//        module.exports = factory(moment, jQuery);
//    } else {
//        // Browser globals
//        root.daterangepicker = factory(root.moment, root.jQuery);
//    }
//}(this, function(moment, $) {
//    var DateRangePicker = function(element, options, cb) {

//        //default settings for options
//        this.parentEl = 'body';
//        this.element = $(element);
//        this.startDate = moment().startOf('day');
//        this.endDate = moment().endOf('day');
//        this.minDate = false;
//        this.maxDate = false;
//        this.dateLimit = false;
//        this.autoApply = false;
//        this.singleDatePicker = false;
//        this.showDropdowns = false;
//        this.showWeekNumbers = false;
//        this.showISOWeekNumbers = false;
//        this.showCustomRangeLabel = true;
//        this.timePicker = false;
//        this.timePicker24Hour = false;
//        this.timePickerIncrement = 1;
//        this.timePickerSeconds = false;
//        this.linkedCalendars = true;
//        this.autoUpdateInput = true;
//        this.alwaysShowCalendars = false;
//        this.ranges = {};

//        this.opens = 'right';
//        if (this.element.hasClass('pull-right'))
//            this.opens = 'left';

//        this.drops = 'down';
//        if (this.element.hasClass('dropup'))
//            this.drops = 'up';

//        this.buttonClasses = 'btn btn-sm';
//        this.applyClass = 'btn-success';
//        this.cancelClass = 'btn-default';

//        this.locale = {
//            direction: 'ltr',
//            format: moment.localeData().longDateFormat('L'),
//            separator: ' - ',
//            applyLabel: 'Apply',
//            startLabel: 'Start date:',
//            endLabel: 'End date:',
//            cancelLabel: 'Cancel',
//            weekLabel: 'W',
//            customRangeLabel: 'Custom Range',
//            daysOfWeek: moment.weekdaysMin(),
//            monthNames: moment.monthsShort(),
//            firstDay: moment.localeData().firstDayOfWeek()
//        };

//        this.callback = function() { };

//        //some state information
//        this.isShowing = false;
//        this.leftCalendar = {};
//        this.rightCalendar = {};

//        //custom options from user
//        if (typeof options !== 'object' || options === null)
//            options = {};

//        //allow setting options with data attributes
//        //data-api options will be overwritten with custom javascript options
//        options = $.extend(this.element.data(), options);

//        //html template for the picker UI
//        if (typeof options.template !== 'string' && !(options.template instanceof $))
//            options.template = '<div class="daterangepicker dropdown-menu">' +
//                '<div class="calendars">' +
//                    '<div class="calendar left">' +
//                        '<div class="calendar-table"></div>' +
//                        '<div class="daterangepicker_input">' +
//                            '<div class="calendar-time">' +
//                                '<div></div>' +
//                            '</div>' +
//                        '</div>' +
//                    '</div>' +

//                    '<div class="calendar right">' +
//                        '<div class="calendar-table"></div>' +
//                        '<div class="daterangepicker_input">' +
//                            '<div class="calendar-time">' +
//                                '<div></div>' +
//                            '</div>' +
//                        '</div>' +
//                    '</div>' +
//                '</div>' +

//                '<div class="ranges">' +
//                    '<div class="daterangepicker-inputs">' +
//                        '<div class="daterangepicker_input">' +
//                            '<span class="start-date-label"></span>' +
//                            '<input class="form-control" type="text" name="daterangepicker_start" value="" />' +
//                            '<i class="icon-calendar3"></i>' +
//                        '</div>' +
//                        '<div class="daterangepicker_input">' +
//                            '<span class="end-date-label"></span>' +
//                            '<input class="form-control" type="text" name="daterangepicker_end" value="" />' +
//                            '<i class="icon-calendar3"></i>' +
//                        '</div>' +
//                    '</div>' +
//                    '<div class="range_inputs">' +
//                        '<button class="applyBtn" disabled="disabled" type="button"></button> ' +
//                        '<button class="cancelBtn" type="button"></button>' +
//                    '</div>' +
//                '</div>' +
//            '</div>';

//        this.parentEl = (options.parentEl && $(options.parentEl).length) ? $(options.parentEl) : $(this.parentEl);
//        this.container = $(options.template).appendTo(this.parentEl);

//        //
//        // handle all the possible options overriding defaults
//        //

//        if (typeof options.locale === 'object') {

//            if (typeof options.locale.direction === 'string')
//                this.locale.direction = options.locale.direction;

//            if (typeof options.locale.format === 'string')
//                this.locale.format = options.locale.format;

//            if (typeof options.locale.separator === 'string')
//                this.locale.separator = options.locale.separator;

//            if (typeof options.locale.daysOfWeek === 'object')
//                this.locale.daysOfWeek = options.locale.daysOfWeek.slice();

//            if (typeof options.locale.monthNames === 'object')
//              this.locale.monthNames = options.locale.monthNames.slice();

//            if (typeof options.locale.firstDay === 'number')
//              this.locale.firstDay = options.locale.firstDay;

//            if (typeof options.locale.applyLabel === 'string')
//              this.locale.applyLabel = options.locale.applyLabel;

//            if (typeof options.locale.startLabel === 'string')
//              this.locale.startLabel = options.locale.startLabel;

//            if (typeof options.locale.endLabel === 'string')
//              this.locale.endLabel = options.locale.endLabel;

//            if (typeof options.locale.cancelLabel === 'string')
//              this.locale.cancelLabel = options.locale.cancelLabel;

//            if (typeof options.locale.weekLabel === 'string')
//              this.locale.weekLabel = options.locale.weekLabel;

//            if (typeof options.locale.customRangeLabel === 'string'){
//                //Support unicode chars in the custom range name.
//                var elem = document.createElement('textarea');
//                elem.innerHTML = options.locale.customRangeLabel;
//                var rangeHtml = elem.value;
//                this.locale.customRangeLabel = rangeHtml;
//            }
//        }
//        this.container.addClass(this.locale.direction);

//        if (typeof options.startDate === 'string')
//            this.startDate = moment(options.startDate, this.locale.format);

//        if (typeof options.endDate === 'string')
//            this.endDate = moment(options.endDate, this.locale.format);

//        if (typeof options.minDate === 'string')
//            this.minDate = moment(options.minDate, this.locale.format);

//        if (typeof options.maxDate === 'string')
//            this.maxDate = moment(options.maxDate, this.locale.format);

//        if (typeof options.startDate === 'object')
//            this.startDate = moment(options.startDate);

//        if (typeof options.endDate === 'object')
//            this.endDate = moment(options.endDate);

//        if (typeof options.minDate === 'object')
//            this.minDate = moment(options.minDate);

//        if (typeof options.maxDate === 'object')
//            this.maxDate = moment(options.maxDate);

//        // sanity check for bad options
//        if (this.minDate && this.startDate.isBefore(this.minDate))
//            this.startDate = this.minDate.clone();

//        // sanity check for bad options
//        if (this.maxDate && this.endDate.isAfter(this.maxDate))
//            this.endDate = this.maxDate.clone();

//        if (typeof options.applyClass === 'string')
//            this.applyClass = options.applyClass;

//        if (typeof options.cancelClass === 'string')
//            this.cancelClass = options.cancelClass;

//        if (typeof options.dateLimit === 'object')
//            this.dateLimit = options.dateLimit;

//        if (typeof options.opens === 'string')
//            this.opens = options.opens;

//        if (typeof options.drops === 'string')
//            this.drops = options.drops;

//        if (typeof options.showWeekNumbers === 'boolean')
//            this.showWeekNumbers = options.showWeekNumbers;

//        if (typeof options.showISOWeekNumbers === 'boolean')
//            this.showISOWeekNumbers = options.showISOWeekNumbers;

//        if (typeof options.buttonClasses === 'string')
//            this.buttonClasses = options.buttonClasses;

//        if (typeof options.buttonClasses === 'object')
//            this.buttonClasses = options.buttonClasses.join(' ');

//        if (typeof options.showDropdowns === 'boolean')
//            this.showDropdowns = options.showDropdowns;

//        if (typeof options.showCustomRangeLabel === 'boolean')
//            this.showCustomRangeLabel = options.showCustomRangeLabel;

//        if (typeof options.singleDatePicker === 'boolean') {
//            this.singleDatePicker = options.singleDatePicker;
//            if (this.singleDatePicker)
//                this.endDate = this.startDate.clone();
//        }

//        if (typeof options.timePicker === 'boolean')
//            this.timePicker = options.timePicker;

//        if (typeof options.timePickerSeconds === 'boolean')
//            this.timePickerSeconds = options.timePickerSeconds;

//        if (typeof options.timePickerIncrement === 'number')
//            this.timePickerIncrement = options.timePickerIncrement;

//        if (typeof options.timePicker24Hour === 'boolean')
//            this.timePicker24Hour = options.timePicker24Hour;

//        if (typeof options.autoApply === 'boolean')
//            this.autoApply = options.autoApply;

//        if (typeof options.autoUpdateInput === 'boolean')
//            this.autoUpdateInput = options.autoUpdateInput;

//        if (typeof options.linkedCalendars === 'boolean')
//            this.linkedCalendars = options.linkedCalendars;

//        if (typeof options.isInvalidDate === 'function')
//            this.isInvalidDate = options.isInvalidDate;

//        if (typeof options.isCustomDate === 'function')
//            this.isCustomDate = options.isCustomDate;

//        if (typeof options.alwaysShowCalendars === 'boolean')
//            this.alwaysShowCalendars = options.alwaysShowCalendars;

//        // update day names order to firstDay
//        if (this.locale.firstDay != 0) {
//            var iterator = this.locale.firstDay;
//            while (iterator > 0) {
//                this.locale.daysOfWeek.push(this.locale.daysOfWeek.shift());
//                iterator--;
//            }
//        }

//        var start, end, range;

//        //if no start/end dates set, check if an input element contains initial values
//        if (typeof options.startDate === 'undefined' && typeof options.endDate === 'undefined') {
//            if ($(this.element).is('input[type=text]')) {
//                var val = $(this.element).val(),
//                    split = val.split(this.locale.separator);

//                start = end = null;

//                if (split.length == 2) {
//                    start = moment(split[0], this.locale.format);
//                    end = moment(split[1], this.locale.format);
//                } else if (this.singleDatePicker && val !== "") {
//                    start = moment(val, this.locale.format);
//                    end = moment(val, this.locale.format);
//                }
//                if (start !== null && end !== null) {
//                    this.setStartDate(start);
//                    this.setEndDate(end);
//                }
//            }
//        }

//        if (typeof options.ranges === 'object') {
//            for (range in options.ranges) {

//                if (typeof options.ranges[range][0] === 'string')
//                    start = moment(options.ranges[range][0], this.locale.format);
//                else
//                    start = moment(options.ranges[range][0]);

//                if (typeof options.ranges[range][1] === 'string')
//                    end = moment(options.ranges[range][1], this.locale.format);
//                else
//                    end = moment(options.ranges[range][1]);

//                // If the start or end date exceed those allowed by the minDate or dateLimit
//                // options, shorten the range to the allowable period.
//                if (this.minDate && start.isBefore(this.minDate))
//                    start = this.minDate.clone();

//                var maxDate = this.maxDate;
//                if (this.dateLimit && maxDate && start.clone().add(this.dateLimit).isAfter(maxDate))
//                    maxDate = start.clone().add(this.dateLimit);
//                if (maxDate && end.isAfter(maxDate))
//                    end = maxDate.clone();

//                // If the end of the range is before the minimum or the start of the range is
//                // after the maximum, don't display this range option at all.
//                if ((this.minDate && end.isBefore(this.minDate, this.timepicker ? 'minute' : 'day')) 
//                  || (maxDate && start.isAfter(maxDate, this.timepicker ? 'minute' : 'day')))
//                    continue;

//                //Support unicode chars in the range names.
//                var elem = document.createElement('textarea');
//                elem.innerHTML = range;
//                var rangeHtml = elem.value;

//                this.ranges[rangeHtml] = [start, end];
//            }

//            var list = '<ul>';
//            for (range in this.ranges) {
//                list += '<li data-range-key="' + range + '">' + range + '</li>';
//            }
//            if (this.showCustomRangeLabel) {
//                list += '<li data-range-key="' + this.locale.customRangeLabel + '">' + this.locale.customRangeLabel + '</li>';
//            }
//            list += '</ul>';
//            this.container.find('.ranges').prepend(list);
//        }

//        if (typeof cb === 'function') {
//            this.callback = cb;
//        }

//        if (!this.timePicker) {
//            this.startDate = this.startDate.startOf('day');
//            this.endDate = this.endDate.endOf('day');
//            this.container.find('.calendar-time').hide();
//        }

//        //can't be used together for now
//        if (this.timePicker && this.autoApply)
//            this.autoApply = false;

//        if (this.autoApply && typeof options.ranges !== 'object') {
//            this.container.find('.ranges').hide();
//        } else if (this.autoApply) {
//            this.container.find('.applyBtn, .cancelBtn').addClass('hide');
//        }

//        if (this.singleDatePicker) {
//            this.container.addClass('single');
//            this.container.find('.calendar.left').addClass('single');
//            this.container.find('.calendar.left').show();
//            this.container.find('.calendar.right').hide();
//            this.container.find('.daterangepicker_input input, .daterangepicker_input > i').hide();
//            if (this.timePicker) {
//                this.container.find('.ranges ul').hide();
//            } else {
//                this.container.find('.ranges').hide();
//            }
//        }

//        if ((typeof options.ranges === 'undefined' && !this.singleDatePicker) || this.alwaysShowCalendars) {
//            this.container.addClass('show-calendar');
//        }

//        this.container.addClass('opens' + this.opens);

//        //swap the position of the predefined ranges if opens right
//        if (typeof options.ranges !== 'undefined' && this.opens == 'right') {
//            this.container.find('.ranges').appendTo( this.container.find('.calendar.left').parent().parent() );
//        }

//        //apply CSS classes and labels to buttons
//        this.container.find('.applyBtn, .cancelBtn').addClass(this.buttonClasses);
//        if (this.applyClass.length)
//            this.container.find('.applyBtn').addClass(this.applyClass);
//        if (this.cancelClass.length)
//            this.container.find('.cancelBtn').addClass(this.cancelClass);
//        this.container.find('.applyBtn').html(this.locale.applyLabel);
//        this.container.find('.cancelBtn').html(this.locale.cancelLabel);

//        //apply CSS classes and labels to text labels
//        this.container.find('.start-date-label').html(this.locale.startLabel);
//        this.container.find('.end-date-label').html(this.locale.endLabel);

//        //
//        // event listeners
//        //

//        this.container.find('.calendar')
//            .on('click.daterangepicker', '.prev', $.proxy(this.clickPrev, this))
//            .on('click.daterangepicker', '.next', $.proxy(this.clickNext, this))
//            .on('mousedown.daterangepicker', 'td.available', $.proxy(this.clickDate, this))
//            .on('mouseenter.daterangepicker', 'td.available', $.proxy(this.hoverDate, this))
//            .on('mouseleave.daterangepicker', 'td.available', $.proxy(this.updateFormInputs, this))
//            .on('change.daterangepicker', 'select.yearselect', $.proxy(this.monthOrYearChanged, this))
//            .on('change.daterangepicker', 'select.monthselect', $.proxy(this.monthOrYearChanged, this))
//            .on('change.daterangepicker', 'select.hourselect,select.minuteselect,select.secondselect,select.ampmselect', $.proxy(this.timeChanged, this))

//        this.container.find('.ranges')
//            .on('click.daterangepicker', '.daterangepicker_input input', $.proxy(this.showCalendars, this))
//            .on('focus.daterangepicker', '.daterangepicker_input input', $.proxy(this.formInputsFocused, this))
//            .on('blur.daterangepicker', '.daterangepicker_input input', $.proxy(this.formInputsBlurred, this))
//            .on('change.daterangepicker', '.daterangepicker_input input', $.proxy(this.formInputsChanged, this))
//            .on('click.daterangepicker', 'button.applyBtn', $.proxy(this.clickApply, this))
//            .on('click.daterangepicker', 'button.cancelBtn', $.proxy(this.clickCancel, this))
//            .on('click.daterangepicker', 'li', $.proxy(this.clickRange, this))
//            .on('mouseenter.daterangepicker', 'li', $.proxy(this.hoverRange, this))
//            .on('mouseleave.daterangepicker', 'li', $.proxy(this.updateFormInputs, this));

//        if (this.element.is('input') || this.element.is('button')) {
//            this.element.on({
//                'click.daterangepicker': $.proxy(this.show, this),
//                'focus.daterangepicker': $.proxy(this.show, this),
//                'keyup.daterangepicker': $.proxy(this.elementChanged, this),
//                'keydown.daterangepicker': $.proxy(this.keydown, this)
//            });
//        } else {
//            this.element.on('click.daterangepicker', $.proxy(this.toggle, this));
//        }

//        //
//        // if attached to a text input, set the initial value
//        //

//        if (this.element.is('input') && !this.singleDatePicker && this.autoUpdateInput) {
//            this.element.val(this.startDate.format(this.locale.format) + this.locale.separator + this.endDate.format(this.locale.format));
//            this.element.trigger('change');
//        } else if (this.element.is('input') && this.autoUpdateInput) {
//            this.element.val(this.startDate.format(this.locale.format));
//            this.element.trigger('change');
//        }

//    };

//    DateRangePicker.prototype = {

//        constructor: DateRangePicker,

//        setStartDate: function(startDate) {
//            if (typeof startDate === 'string')
//                this.startDate = moment(startDate, this.locale.format);

//            if (typeof startDate === 'object')
//                this.startDate = moment(startDate);

//            if (!this.timePicker)
//                this.startDate = this.startDate.startOf('day');

//            if (this.timePicker && this.timePickerIncrement)
//                this.startDate.minute(Math.round(this.startDate.minute() / this.timePickerIncrement) * this.timePickerIncrement);

//            if (this.minDate && this.startDate.isBefore(this.minDate)) {
//                this.startDate = this.minDate.clone();
//                if (this.timePicker && this.timePickerIncrement)
//                    this.startDate.minute(Math.round(this.startDate.minute() / this.timePickerIncrement) * this.timePickerIncrement);
//            }

//            if (this.maxDate && this.startDate.isAfter(this.maxDate)) {
//                this.startDate = this.maxDate.clone();
//                if (this.timePicker && this.timePickerIncrement)
//                    this.startDate.minute(Math.floor(this.startDate.minute() / this.timePickerIncrement) * this.timePickerIncrement);
//            }

//            if (!this.isShowing)
//                this.updateElement();

//            this.updateMonthsInView();
//        },

//        setEndDate: function(endDate) {
//            if (typeof endDate === 'string')
//                this.endDate = moment(endDate, this.locale.format);

//            if (typeof endDate === 'object')
//                this.endDate = moment(endDate);

//            if (!this.timePicker)
//                this.endDate = this.endDate.endOf('day');

//            if (this.timePicker && this.timePickerIncrement)
//                this.endDate.minute(Math.round(this.endDate.minute() / this.timePickerIncrement) * this.timePickerIncrement);

//            if (this.endDate.isBefore(this.startDate))
//                this.endDate = this.startDate.clone();

//            if (this.maxDate && this.endDate.isAfter(this.maxDate))
//                this.endDate = this.maxDate.clone();

//            if (this.dateLimit && this.startDate.clone().add(this.dateLimit).isBefore(this.endDate))
//                this.endDate = this.startDate.clone().add(this.dateLimit);

//            this.previousRightTime = this.endDate.clone();

//            if (!this.isShowing)
//                this.updateElement();

//            this.updateMonthsInView();
//        },

//        isInvalidDate: function() {
//            return false;
//        },

//        isCustomDate: function() {
//            return false;
//        },

//        updateView: function() {
//            if (this.timePicker) {
//                this.renderTimePicker('left');
//                this.renderTimePicker('right');
//                if (!this.endDate) {
//                    this.container.find('.right .calendar-time select').attr('disabled', 'disabled').addClass('disabled');
//                } else {
//                    this.container.find('.right .calendar-time select').removeAttr('disabled').removeClass('disabled');
//                }
//            }
//            if (this.endDate) {
//                this.container.find('input[name="daterangepicker_end"]').removeClass('active');
//                this.container.find('input[name="daterangepicker_start"]').addClass('active');
//            } else {
//                this.container.find('input[name="daterangepicker_end"]').addClass('active');
//                this.container.find('input[name="daterangepicker_start"]').removeClass('active');
//            }
//            this.updateMonthsInView();
//            this.updateCalendars();
//            this.updateFormInputs();
//        },

//        updateMonthsInView: function() {
//            if (this.endDate) {

//                //if both dates are visible already, do nothing
//                if (!this.singleDatePicker && this.leftCalendar.month && this.rightCalendar.month &&
//                    (this.startDate.format('YYYY-MM') == this.leftCalendar.month.format('YYYY-MM') || this.startDate.format('YYYY-MM') == this.rightCalendar.month.format('YYYY-MM'))
//                    &&
//                    (this.endDate.format('YYYY-MM') == this.leftCalendar.month.format('YYYY-MM') || this.endDate.format('YYYY-MM') == this.rightCalendar.month.format('YYYY-MM'))
//                    ) {
//                    return;
//                }

//                this.leftCalendar.month = this.startDate.clone().date(2);
//                if (!this.linkedCalendars && (this.endDate.month() != this.startDate.month() || this.endDate.year() != this.startDate.year())) {
//                    this.rightCalendar.month = this.endDate.clone().date(2);
//                } else {
//                    this.rightCalendar.month = this.startDate.clone().date(2).add(1, 'month');
//                }

//            } else {
//                if (this.leftCalendar.month.format('YYYY-MM') != this.startDate.format('YYYY-MM') && this.rightCalendar.month.format('YYYY-MM') != this.startDate.format('YYYY-MM')) {
//                    this.leftCalendar.month = this.startDate.clone().date(2);
//                    this.rightCalendar.month = this.startDate.clone().date(2).add(1, 'month');
//                }
//            }
//            if (this.maxDate && this.linkedCalendars && !this.singleDatePicker && this.rightCalendar.month > this.maxDate) {
//              this.rightCalendar.month = this.maxDate.clone().date(2);
//              this.leftCalendar.month = this.maxDate.clone().date(2).subtract(1, 'month');
//            }
//        },

//        updateCalendars: function() {

//            if (this.timePicker) {
//                var hour, minute, second;
//                if (this.endDate) {
//                    hour = parseInt(this.container.find('.left .hourselect').val(), 10);
//                    minute = parseInt(this.container.find('.left .minuteselect').val(), 10);
//                    second = this.timePickerSeconds ? parseInt(this.container.find('.left .secondselect').val(), 10) : 0;
//                    if (!this.timePicker24Hour) {
//                        var ampm = this.container.find('.left .ampmselect').val();
//                        if (ampm === 'PM' && hour < 12)
//                            hour += 12;
//                        if (ampm === 'AM' && hour === 12)
//                            hour = 0;
//                    }
//                } else {
//                    hour = parseInt(this.container.find('.right .hourselect').val(), 10);
//                    minute = parseInt(this.container.find('.right .minuteselect').val(), 10);
//                    second = this.timePickerSeconds ? parseInt(this.container.find('.right .secondselect').val(), 10) : 0;
//                    if (!this.timePicker24Hour) {
//                        var ampm = this.container.find('.right .ampmselect').val();
//                        if (ampm === 'PM' && hour < 12)
//                            hour += 12;
//                        if (ampm === 'AM' && hour === 12)
//                            hour = 0;
//                    }
//                }
//                this.leftCalendar.month.hour(hour).minute(minute).second(second);
//                this.rightCalendar.month.hour(hour).minute(minute).second(second);
//            }

//            this.renderCalendar('left');
//            this.renderCalendar('right');

//            //highlight any predefined range matching the current start and end dates
//            this.container.find('.ranges li').removeClass('active');
//            if (this.endDate == null) return;

//            this.calculateChosenLabel();
//        },

//        renderCalendar: function(side) {

//            //
//            // Build the matrix of dates that will populate the calendar
//            //

//            var calendar = side == 'left' ? this.leftCalendar : this.rightCalendar;
//            var month = calendar.month.month();
//            var year = calendar.month.year();
//            var hour = calendar.month.hour();
//            var minute = calendar.month.minute();
//            var second = calendar.month.second();
//            var daysInMonth = moment([year, month]).daysInMonth();
//            var firstDay = moment([year, month, 1]);
//            var lastDay = moment([year, month, daysInMonth]);
//            var lastMonth = moment(firstDay).subtract(1, 'month').month();
//            var lastYear = moment(firstDay).subtract(1, 'month').year();
//            var daysInLastMonth = moment([lastYear, lastMonth]).daysInMonth();
//            var dayOfWeek = firstDay.day();

//            //initialize a 6 rows x 7 columns array for the calendar
//            var calendar = [];
//            calendar.firstDay = firstDay;
//            calendar.lastDay = lastDay;

//            for (var i = 0; i < 6; i++) {
//                calendar[i] = [];
//            }

//            //populate the calendar with date objects
//            var startDay = daysInLastMonth - dayOfWeek + this.locale.firstDay + 1;
//            if (startDay > daysInLastMonth)
//                startDay -= 7;

//            if (dayOfWeek == this.locale.firstDay)
//                startDay = daysInLastMonth - 6;

//            var curDate = moment([lastYear, lastMonth, startDay, 12, minute, second]);

//            var col, row;
//            for (var i = 0, col = 0, row = 0; i < 42; i++, col++, curDate = moment(curDate).add(24, 'hour')) {
//                if (i > 0 && col % 7 === 0) {
//                    col = 0;
//                    row++;
//                }
//                calendar[row][col] = curDate.clone().hour(hour).minute(minute).second(second);
//                curDate.hour(12);

//                if (this.minDate && calendar[row][col].format('YYYY-MM-DD') == this.minDate.format('YYYY-MM-DD') && calendar[row][col].isBefore(this.minDate) && side == 'left') {
//                    calendar[row][col] = this.minDate.clone();
//                }

//                if (this.maxDate && calendar[row][col].format('YYYY-MM-DD') == this.maxDate.format('YYYY-MM-DD') && calendar[row][col].isAfter(this.maxDate) && side == 'right') {
//                    calendar[row][col] = this.maxDate.clone();
//                }

//            }

//            //make the calendar object available to hoverDate/clickDate
//            if (side == 'left') {
//                this.leftCalendar.calendar = calendar;
//            } else {
//                this.rightCalendar.calendar = calendar;
//            }

//            //
//            // Display the calendar
//            //

//            var minDate = side == 'left' ? this.minDate : this.startDate;
//            var maxDate = this.maxDate;
//            var selected = side == 'left' ? this.startDate : this.endDate;
//            var arrow = this.locale.direction == 'ltr' ? {left: 'arrow-left32', right: 'arrow-right32'} : {left: 'arrow-right32', right: 'arrow-left32'};

//            var html = '<table class="table-condensed">';
//            html += '<thead>';
//            html += '<tr>';

//            // add empty cell for week number
//            if (this.showWeekNumbers || this.showISOWeekNumbers)
//                html += '<th></th>';

//            if ((!minDate || minDate.isBefore(calendar.firstDay)) && (!this.linkedCalendars || side == 'left')) {
//                html += '<th class="prev available"><i class="icon-' + arrow.left + '"></i></th>';
//            } else {
//                html += '<th></th>';
//            }

//            var dateHtml = this.locale.monthNames[calendar[1][1].month()] + calendar[1][1].format(" YYYY");

//            if (this.showDropdowns) {
//                var currentMonth = calendar[1][1].month();
//                var currentYear = calendar[1][1].year();
//                var maxYear = (maxDate && maxDate.year()) || (currentYear + 5);
//                var minYear = (minDate && minDate.year()) || (currentYear - 50);
//                var inMinYear = currentYear == minYear;
//                var inMaxYear = currentYear == maxYear;

//                var monthHtml = '<select class="monthselect form-control input-sm">';
//                for (var m = 0; m < 12; m++) {
//                    if ((!inMinYear || m >= minDate.month()) && (!inMaxYear || m <= maxDate.month())) {
//                        monthHtml += "<option value='" + m + "'" +
//                            (m === currentMonth ? " selected='selected'" : "") +
//                            ">" + this.locale.monthNames[m] + "</option>";
//                    } else {
//                        monthHtml += "<option value='" + m + "'" +
//                            (m === currentMonth ? " selected='selected'" : "") +
//                            " disabled='disabled'>" + this.locale.monthNames[m] + "</option>";
//                    }
//                }
//                monthHtml += "</select>";

//                var yearHtml = '<select class="yearselect form-control input-sm">';
//                for (var y = minYear; y <= maxYear; y++) {
//                    yearHtml += '<option value="' + y + '"' +
//                        (y === currentYear ? ' selected="selected"' : '') +
//                        '>' + y + '</option>';
//                }
//                yearHtml += '</select>';

//                dateHtml = monthHtml + yearHtml;
//            }

//            html += '<th colspan="5" class="month">' + dateHtml + '</th>';
//            if ((!maxDate || maxDate.isAfter(calendar.lastDay)) && (!this.linkedCalendars || side == 'right' || this.singleDatePicker)) {
//                html += '<th class="next available"><i class="icon-' + arrow.right + '"></i></th>';
//            } else {
//                html += '<th></th>';
//            }

//            html += '</tr>';
//            html += '<tr>';

//            // add week number label
//            if (this.showWeekNumbers || this.showISOWeekNumbers)
//                html += '<th class="week">' + this.locale.weekLabel + '</th>';

//            $.each(this.locale.daysOfWeek, function(index, dayOfWeek) {
//                html += '<th>' + dayOfWeek + '</th>';
//            });

//            html += '</tr>';
//            html += '</thead>';
//            html += '<tbody>';

//            //adjust maxDate to reflect the dateLimit setting in order to
//            //grey out end dates beyond the dateLimit
//            if (this.endDate == null && this.dateLimit) {
//                var maxLimit = this.startDate.clone().add(this.dateLimit).endOf('day');
//                if (!maxDate || maxLimit.isBefore(maxDate)) {
//                    maxDate = maxLimit;
//                }
//            }

//            for (var row = 0; row < 6; row++) {
//                html += '<tr>';

//                // add week number
//                if (this.showWeekNumbers)
//                    html += '<td class="week">' + calendar[row][0].week() + '</td>';
//                else if (this.showISOWeekNumbers)
//                    html += '<td class="week">' + calendar[row][0].isoWeek() + '</td>';

//                for (var col = 0; col < 7; col++) {

//                    var classes = [];

//                    //highlight today's date
//                    if (calendar[row][col].isSame(new Date(), "day"))
//                        classes.push('today');

//                    //highlight weekends
//                    if (calendar[row][col].isoWeekday() > 5)
//                        classes.push('weekend');

//                    //grey out the dates in other months displayed at beginning and end of this calendar
//                    if (calendar[row][col].month() != calendar[1][1].month())
//                        classes.push('off');

//                    //don't allow selection of dates before the minimum date
//                    if (this.minDate && calendar[row][col].isBefore(this.minDate, 'day'))
//                        classes.push('off', 'disabled');

//                    //don't allow selection of dates after the maximum date
//                    if (maxDate && calendar[row][col].isAfter(maxDate, 'day'))
//                        classes.push('off', 'disabled');

//                    //don't allow selection of date if a custom function decides it's invalid
//                    if (this.isInvalidDate(calendar[row][col]))
//                        classes.push('off', 'disabled');

//                    //highlight the currently selected start date
//                    if (calendar[row][col].format('YYYY-MM-DD') == this.startDate.format('YYYY-MM-DD'))
//                        classes.push('active', 'start-date');

//                    //highlight the currently selected end date
//                    if (this.endDate != null && calendar[row][col].format('YYYY-MM-DD') == this.endDate.format('YYYY-MM-DD'))
//                        classes.push('active', 'end-date');

//                    //highlight dates in-between the selected dates
//                    if (this.endDate != null && calendar[row][col] > this.startDate && calendar[row][col] < this.endDate)
//                        classes.push('in-range');

//                    //apply custom classes for this date
//                    var isCustom = this.isCustomDate(calendar[row][col]);
//                    if (isCustom !== false) {
//                        if (typeof isCustom === 'string')
//                            classes.push(isCustom);
//                        else
//                            Array.prototype.push.apply(classes, isCustom);
//                    }

//                    var cname = '', disabled = false;
//                    for (var i = 0; i < classes.length; i++) {
//                        cname += classes[i] + ' ';
//                        if (classes[i] == 'disabled')
//                            disabled = true;
//                    }
//                    if (!disabled)
//                        cname += 'available';

//                    html += '<td class="' + cname.replace(/^\s+|\s+$/g, '') + '" data-title="' + 'r' + row + 'c' + col + '">' + calendar[row][col].date() + '</td>';

//                }
//                html += '</tr>';
//            }

//            html += '</tbody>';
//            html += '</table>';

//            this.container.find('.calendar.' + side + ' .calendar-table').html(html);

//        },

//        renderTimePicker: function(side) {

//            // Don't bother updating the time picker if it's currently disabled
//            // because an end date hasn't been clicked yet
//            if (side == 'right' && !this.endDate) return;

//            var html, selected, minDate, maxDate = this.maxDate;

//            if (this.dateLimit && (!this.maxDate || this.startDate.clone().add(this.dateLimit).isAfter(this.maxDate)))
//                maxDate = this.startDate.clone().add(this.dateLimit);

//            if (side == 'left') {
//                selected = this.startDate.clone();
//                minDate = this.minDate;
//            } else if (side == 'right') {
//                selected = this.endDate.clone();
//                minDate = this.startDate;

//                //Preserve the time already selected
//                var timeSelector = this.container.find('.calendar.right .calendar-time div');
//                if (timeSelector.html() != '') {

//                    selected.hour(timeSelector.find('.hourselect option:selected').val() || selected.hour());
//                    selected.minute(timeSelector.find('.minuteselect option:selected').val() || selected.minute());
//                    selected.second(timeSelector.find('.secondselect option:selected').val() || selected.second());

//                    if (!this.timePicker24Hour) {
//                        var ampm = timeSelector.find('.ampmselect option:selected').val();
//                        if (ampm === 'PM' && selected.hour() < 12)
//                            selected.hour(selected.hour() + 12);
//                        if (ampm === 'AM' && selected.hour() === 12)
//                            selected.hour(0);
//                    }

//                }

//                if (selected.isBefore(this.startDate))
//                    selected = this.startDate.clone();

//                if (maxDate && selected.isAfter(maxDate))
//                    selected = maxDate.clone();

//            }

//            //
//            // hours
//            //

//            html = '<select class="hourselect form-control input-sm">';

//            var start = this.timePicker24Hour ? 0 : 1;
//            var end = this.timePicker24Hour ? 23 : 12;

//            for (var i = start; i <= end; i++) {
//                var i_in_24 = i;
//                if (!this.timePicker24Hour)
//                    i_in_24 = selected.hour() >= 12 ? (i == 12 ? 12 : i + 12) : (i == 12 ? 0 : i);

//                var time = selected.clone().hour(i_in_24);
//                var disabled = false;
//                if (minDate && time.minute(59).isBefore(minDate))
//                    disabled = true;
//                if (maxDate && time.minute(0).isAfter(maxDate))
//                    disabled = true;

//                if (i_in_24 == selected.hour() && !disabled) {
//                    html += '<option value="' + i + '" selected="selected">' + i + '</option>';
//                } else if (disabled) {
//                    html += '<option value="' + i + '" disabled="disabled" class="disabled">' + i + '</option>';
//                } else {
//                    html += '<option value="' + i + '">' + i + '</option>';
//                }
//            }

//            html += '</select> ';

//            //
//            // minutes
//            //

//            html += ': <select class="minuteselect form-control input-sm">';

//            for (var i = 0; i < 60; i += this.timePickerIncrement) {
//                var padded = i < 10 ? '0' + i : i;
//                var time = selected.clone().minute(i);

//                var disabled = false;
//                if (minDate && time.second(59).isBefore(minDate))
//                    disabled = true;
//                if (maxDate && time.second(0).isAfter(maxDate))
//                    disabled = true;

//                if (selected.minute() == i && !disabled) {
//                    html += '<option value="' + i + '" selected="selected">' + padded + '</option>';
//                } else if (disabled) {
//                    html += '<option value="' + i + '" disabled="disabled" class="disabled">' + padded + '</option>';
//                } else {
//                    html += '<option value="' + i + '">' + padded + '</option>';
//                }
//            }

//            html += '</select> ';

//            //
//            // seconds
//            //

//            if (this.timePickerSeconds) {
//                html += ': <select class="secondselect form-control input-sm">';

//                for (var i = 0; i < 60; i++) {
//                    var padded = i < 10 ? '0' + i : i;
//                    var time = selected.clone().second(i);

//                    var disabled = false;
//                    if (minDate && time.isBefore(minDate))
//                        disabled = true;
//                    if (maxDate && time.isAfter(maxDate))
//                        disabled = true;

//                    if (selected.second() == i && !disabled) {
//                        html += '<option value="' + i + '" selected="selected">' + padded + '</option>';
//                    } else if (disabled) {
//                        html += '<option value="' + i + '" disabled="disabled" class="disabled">' + padded + '</option>';
//                    } else {
//                        html += '<option value="' + i + '">' + padded + '</option>';
//                    }
//                }

//                html += '</select> ';
//            }

//            //
//            // AM/PM
//            //

//            if (!this.timePicker24Hour) {
//                html += '<select class="ampmselect form-control input-sm">';

//                var am_html = '';
//                var pm_html = '';

//                if (minDate && selected.clone().hour(12).minute(0).second(0).isBefore(minDate))
//                    am_html = ' disabled="disabled" class="disabled"';

//                if (maxDate && selected.clone().hour(0).minute(0).second(0).isAfter(maxDate))
//                    pm_html = ' disabled="disabled" class="disabled"';

//                if (selected.hour() >= 12) {
//                    html += '<option value="AM"' + am_html + '>AM</option><option value="PM" selected="selected"' + pm_html + '>PM</option>';
//                } else {
//                    html += '<option value="AM" selected="selected"' + am_html + '>AM</option><option value="PM"' + pm_html + '>PM</option>';
//                }

//                html += '</select>';
//            }

//            this.container.find('.calendar.' + side + ' .calendar-time div').html(html);

//        },

//        updateFormInputs: function() {

//            //ignore mouse movements while an above-calendar text input has focus
//            if (this.container.find('input[name=daterangepicker_start]').is(":focus") || this.container.find('input[name=daterangepicker_end]').is(":focus"))
//                return;

//            this.container.find('input[name=daterangepicker_start]').val(this.startDate.format(this.locale.format));
//            if (this.endDate)
//                this.container.find('input[name=daterangepicker_end]').val(this.endDate.format(this.locale.format));

//            if (this.singleDatePicker || (this.endDate && (this.startDate.isBefore(this.endDate) || this.startDate.isSame(this.endDate)))) {
//                this.container.find('button.applyBtn').removeAttr('disabled');
//            } else {
//                this.container.find('button.applyBtn').attr('disabled', 'disabled');
//            }

//        },

//        move: function() {
//            var parentOffset = { top: 0, left: 0 },
//                containerTop;
//            var parentRightEdge = $(window).width();
//            if (!this.parentEl.is('body')) {
//                parentOffset = {
//                    top: this.parentEl.offset().top - this.parentEl.scrollTop(),
//                    left: this.parentEl.offset().left - this.parentEl.scrollLeft()
//                };
//                parentRightEdge = this.parentEl[0].clientWidth + this.parentEl.offset().left;
//            }

//            if (this.drops == 'up')
//                containerTop = this.element.offset().top - this.container.outerHeight() - parentOffset.top;
//            else
//                containerTop = this.element.offset().top + this.element.outerHeight() - parentOffset.top;
//            this.container[this.drops == 'up' ? 'addClass' : 'removeClass']('dropup');

//            if (this.opens == 'left') {
//                this.container.css({
//                    top: containerTop,
//                    right: parentRightEdge - this.element.offset().left - this.element.outerWidth(),
//                    left: 'auto'
//                });
//                if (this.container.offset().left < 0) {
//                    this.container.css({
//                        right: 'auto',
//                        left: 9
//                    });
//                }
//            } else if (this.opens == 'center') {
//                this.container.css({
//                    top: containerTop,
//                    left: this.element.offset().left - parentOffset.left + this.element.outerWidth() / 2
//                            - this.container.outerWidth() / 2,
//                    right: 'auto'
//                });
//                if (this.container.offset().left < 0) {
//                    this.container.css({
//                        right: 'auto',
//                        left: 9
//                    });
//                }
//            } else {
//                this.container.css({
//                    top: containerTop,
//                    left: this.element.offset().left - parentOffset.left,
//                    right: 'auto'
//                });
//                if (this.container.offset().left + this.container.outerWidth() > $(window).width()) {
//                    this.container.css({
//                        left: 'auto',
//                        right: 0
//                    });
//                }
//            }
//        },

//        show: function(e) {
//            if (this.isShowing) return;

//            // Create a click proxy that is private to this instance of datepicker, for unbinding
//            this._outsideClickProxy = $.proxy(function(e) { this.outsideClick(e); }, this);

//            // Bind global datepicker mousedown for hiding and
//            $(document)
//              .on('mousedown.daterangepicker', this._outsideClickProxy)
//              // also support mobile devices
//              .on('touchend.daterangepicker', this._outsideClickProxy)
//              // also explicitly play nice with Bootstrap dropdowns, which stopPropagation when clicking them
//              .on('click.daterangepicker', '[data-toggle=dropdown]', this._outsideClickProxy)
//              // and also close when focus changes to outside the picker (eg. tabbing between controls)
//              .on('focusin.daterangepicker', this._outsideClickProxy);

//            // Reposition the picker if the window is resized while it's open
//            $(window).on('resize.daterangepicker', $.proxy(function(e) { this.move(e); }, this));

//            this.oldStartDate = this.startDate.clone();
//            this.oldEndDate = this.endDate.clone();
//            this.previousRightTime = this.endDate.clone();

//            this.updateView();
//            this.container.show();
//            this.move();
//            this.element.trigger('show.daterangepicker', this);
//            this.isShowing = true;
//        },

//        hide: function(e) {
//            if (!this.isShowing) return;

//            //incomplete date selection, revert to last values
//            if (!this.endDate) {
//                this.startDate = this.oldStartDate.clone();
//                this.endDate = this.oldEndDate.clone();
//            }

//            //if a new date range was selected, invoke the user callback function
//            if (!this.startDate.isSame(this.oldStartDate) || !this.endDate.isSame(this.oldEndDate))
//                this.callback(this.startDate, this.endDate, this.chosenLabel);

//            //if picker is attached to a text input, update it
//            this.updateElement();

//            $(document).off('.daterangepicker');
//            $(window).off('.daterangepicker');
//            this.container.hide();
//            this.element.trigger('hide.daterangepicker', this);
//            this.isShowing = false;
//        },

//        toggle: function(e) {
//            if (this.isShowing) {
//                this.hide();
//            } else {
//                this.show();
//            }
//        },

//        outsideClick: function(e) {
//            var target = $(e.target);
//            // if the page is clicked anywhere except within the daterangerpicker/button
//            // itself then call this.hide()
//            if (
//                // ie modal dialog fix
//                e.type == "focusin" ||
//                target.closest(this.element).length ||
//                target.closest(this.container).length ||
//                target.closest('.calendar-table').length
//                ) return;
//            this.hide();
//            this.element.trigger('outsideClick.daterangepicker', this);
//        },

//        showCalendars: function() {
//            this.container.addClass('show-calendar');
//            this.move();
//            this.element.trigger('showCalendar.daterangepicker', this);
//        },

//        hideCalendars: function() {
//            this.container.removeClass('show-calendar');
//            this.element.trigger('hideCalendar.daterangepicker', this);
//        },

//        hoverRange: function(e) {

//            //ignore mouse movements while an above-calendar text input has focus
//            if (this.container.find('input[name=daterangepicker_start]').is(":focus") || this.container.find('input[name=daterangepicker_end]').is(":focus"))
//                return;

//            var label = e.target.getAttribute('data-range-key');

//            if (label == this.locale.customRangeLabel) {
//                this.updateView();
//            } else {
//                var dates = this.ranges[label];
//                this.container.find('input[name=daterangepicker_start]').val(dates[0].format(this.locale.format));
//                this.container.find('input[name=daterangepicker_end]').val(dates[1].format(this.locale.format));
//            }

//        },

//        clickRange: function(e) {
//            var label = e.target.getAttribute('data-range-key');
//            this.chosenLabel = label;
//            if (label == this.locale.customRangeLabel) {
//                this.showCalendars();
//            } else {
//                var dates = this.ranges[label];
//                this.startDate = dates[0];
//                this.endDate = dates[1];

//                if (!this.timePicker) {
//                    this.startDate.startOf('day');
//                    this.endDate.endOf('day');
//                }

//                if (!this.alwaysShowCalendars)
//                    this.hideCalendars();
//                this.clickApply();
//            }
//        },

//        clickPrev: function(e) {
//            var cal = $(e.target).parents('.calendar');
//            if (cal.hasClass('left')) {
//                this.leftCalendar.month.subtract(1, 'month');
//                if (this.linkedCalendars)
//                    this.rightCalendar.month.subtract(1, 'month');
//            } else {
//                this.rightCalendar.month.subtract(1, 'month');
//            }
//            this.updateCalendars();
//        },

//        clickNext: function(e) {
//            var cal = $(e.target).parents('.calendar');
//            if (cal.hasClass('left')) {
//                this.leftCalendar.month.add(1, 'month');
//            } else {
//                this.rightCalendar.month.add(1, 'month');
//                if (this.linkedCalendars)
//                    this.leftCalendar.month.add(1, 'month');
//            }
//            this.updateCalendars();
//        },

//        hoverDate: function(e) {

//            //ignore mouse movements while an above-calendar text input has focus
//            //if (this.container.find('input[name=daterangepicker_start]').is(":focus") || this.container.find('input[name=daterangepicker_end]').is(":focus"))
//            //    return;

//            //ignore dates that can't be selected
//            if (!$(e.target).hasClass('available')) return;

//            //have the text inputs above calendars reflect the date being hovered over
//            var title = $(e.target).attr('data-title');
//            var row = title.substr(1, 1);
//            var col = title.substr(3, 1);
//            var cal = $(e.target).parents('.calendar');
//            var date = cal.hasClass('left') ? this.leftCalendar.calendar[row][col] : this.rightCalendar.calendar[row][col];

//            if (this.endDate && !this.container.find('input[name=daterangepicker_start]').is(":focus")) {
//                this.container.find('input[name=daterangepicker_start]').val(date.format(this.locale.format));
//            } else if (!this.endDate && !this.container.find('input[name=daterangepicker_end]').is(":focus")) {
//                this.container.find('input[name=daterangepicker_end]').val(date.format(this.locale.format));
//            }

//            //highlight the dates between the start date and the date being hovered as a potential end date
//            var leftCalendar = this.leftCalendar;
//            var rightCalendar = this.rightCalendar;
//            var startDate = this.startDate;
//            if (!this.endDate) {
//                this.container.find('.calendar tbody td').each(function(index, el) {

//                    //skip week numbers, only look at dates
//                    if ($(el).hasClass('week')) return;

//                    var title = $(el).attr('data-title');
//                    var row = title.substr(1, 1);
//                    var col = title.substr(3, 1);
//                    var cal = $(el).parents('.calendar');
//                    var dt = cal.hasClass('left') ? leftCalendar.calendar[row][col] : rightCalendar.calendar[row][col];

//                    if ((dt.isAfter(startDate) && dt.isBefore(date)) || dt.isSame(date, 'day')) {
//                        $(el).addClass('in-range');
//                    } else {
//                        $(el).removeClass('in-range');
//                    }

//                });
//            }

//        },

//        clickDate: function(e) {

//            if (!$(e.target).hasClass('available')) return;

//            var title = $(e.target).attr('data-title');
//            var row = title.substr(1, 1);
//            var col = title.substr(3, 1);
//            var cal = $(e.target).parents('.calendar');
//            var date = cal.hasClass('left') ? this.leftCalendar.calendar[row][col] : this.rightCalendar.calendar[row][col];

//            //
//            // this function needs to do a few things:
//            // * alternate between selecting a start and end date for the range,
//            // * if the time picker is enabled, apply the hour/minute/second from the select boxes to the clicked date
//            // * if autoapply is enabled, and an end date was chosen, apply the selection
//            // * if single date picker mode, and time picker isn't enabled, apply the selection immediately
//            // * if one of the inputs above the calendars was focused, cancel that manual input
//            //

//            if (this.endDate || date.isBefore(this.startDate, 'day')) { //picking start
//                if (this.timePicker) {
//                    var hour = parseInt(this.container.find('.left .hourselect').val(), 10);
//                    if (!this.timePicker24Hour) {
//                        var ampm = this.container.find('.left .ampmselect').val();
//                        if (ampm === 'PM' && hour < 12)
//                            hour += 12;
//                        if (ampm === 'AM' && hour === 12)
//                            hour = 0;
//                    }
//                    var minute = parseInt(this.container.find('.left .minuteselect').val(), 10);
//                    var second = this.timePickerSeconds ? parseInt(this.container.find('.left .secondselect').val(), 10) : 0;
//                    date = date.clone().hour(hour).minute(minute).second(second);
//                }
//                this.endDate = null;
//                this.setStartDate(date.clone());
//            } else if (!this.endDate && date.isBefore(this.startDate)) {
//                //special case: clicking the same date for start/end,
//                //but the time of the end date is before the start date
//                this.setEndDate(this.startDate.clone());
//            } else { // picking end
//                if (this.timePicker) {
//                    var hour = parseInt(this.container.find('.right .hourselect').val(), 10);
//                    if (!this.timePicker24Hour) {
//                        var ampm = this.container.find('.right .ampmselect').val();
//                        if (ampm === 'PM' && hour < 12)
//                            hour += 12;
//                        if (ampm === 'AM' && hour === 12)
//                            hour = 0;
//                    }
//                    var minute = parseInt(this.container.find('.right .minuteselect').val(), 10);
//                    var second = this.timePickerSeconds ? parseInt(this.container.find('.right .secondselect').val(), 10) : 0;
//                    date = date.clone().hour(hour).minute(minute).second(second);
//                }
//                this.setEndDate(date.clone());
//                if (this.autoApply) {
//                  this.calculateChosenLabel();
//                  this.clickApply();
//                }
//            }

//            if (this.singleDatePicker) {
//                this.setEndDate(this.startDate);
//                if (!this.timePicker)
//                    this.clickApply();
//            }

//            this.updateView();

//            //This is to cancel the blur event handler if the mouse was in one of the inputs
//            e.stopPropagation();

//        },

//        calculateChosenLabel: function () {
//            var customRange = true;
//            var i = 0;
//            for (var range in this.ranges) {
//                if (this.timePicker) {
//                    if (this.startDate.isSame(this.ranges[range][0]) && this.endDate.isSame(this.ranges[range][1])) {
//                        customRange = false;
//                        this.chosenLabel = this.container.find('.ranges li:eq(' + i + ')').addClass('active').html();
//                        break;
//                    }
//                } else {
//                    //ignore times when comparing dates if time picker is not enabled
//                    if (this.startDate.format('YYYY-MM-DD') == this.ranges[range][0].format('YYYY-MM-DD') && this.endDate.format('YYYY-MM-DD') == this.ranges[range][1].format('YYYY-MM-DD')) {
//                        customRange = false;
//                        this.chosenLabel = this.container.find('.ranges li:eq(' + i + ')').addClass('active').html();
//                        break;
//                    }
//                }
//                i++;
//            }
//            if (customRange) {
//                if (this.showCustomRangeLabel) {
//                    this.chosenLabel = this.container.find('.ranges li:last').addClass('active').html();
//                } else {
//                    this.chosenLabel = null;
//                }
//                this.showCalendars();
//            }
//        },

//        clickApply: function(e) {
//            this.hide();
//            this.element.trigger('apply.daterangepicker', this);
//        },

//        clickCancel: function(e) {
//            this.startDate = this.oldStartDate;
//            this.endDate = this.oldEndDate;
//            this.hide();
//            this.element.trigger('cancel.daterangepicker', this);
//        },

//        monthOrYearChanged: function(e) {
//            var isLeft = $(e.target).closest('.calendar').hasClass('left'),
//                leftOrRight = isLeft ? 'left' : 'right',
//                cal = this.container.find('.calendar.'+leftOrRight);

//            // Month must be Number for new moment versions
//            var month = parseInt(cal.find('.monthselect').val(), 10);
//            var year = cal.find('.yearselect').val();

//            if (!isLeft) {
//                if (year < this.startDate.year() || (year == this.startDate.year() && month < this.startDate.month())) {
//                    month = this.startDate.month();
//                    year = this.startDate.year();
//                }
//            }

//            if (this.minDate) {
//                if (year < this.minDate.year() || (year == this.minDate.year() && month < this.minDate.month())) {
//                    month = this.minDate.month();
//                    year = this.minDate.year();
//                }
//            }

//            if (this.maxDate) {
//                if (year > this.maxDate.year() || (year == this.maxDate.year() && month > this.maxDate.month())) {
//                    month = this.maxDate.month();
//                    year = this.maxDate.year();
//                }
//            }

//            if (isLeft) {
//                this.leftCalendar.month.month(month).year(year);
//                if (this.linkedCalendars)
//                    this.rightCalendar.month = this.leftCalendar.month.clone().add(1, 'month');
//            } else {
//                this.rightCalendar.month.month(month).year(year);
//                if (this.linkedCalendars)
//                    this.leftCalendar.month = this.rightCalendar.month.clone().subtract(1, 'month');
//            }
//            this.updateCalendars();
//        },

//        timeChanged: function(e) {

//            var cal = $(e.target).closest('.calendar'),
//                isLeft = cal.hasClass('left');

//            var hour = parseInt(cal.find('.hourselect').val(), 10);
//            var minute = parseInt(cal.find('.minuteselect').val(), 10);
//            var second = this.timePickerSeconds ? parseInt(cal.find('.secondselect').val(), 10) : 0;

//            if (!this.timePicker24Hour) {
//                var ampm = cal.find('.ampmselect').val();
//                if (ampm === 'PM' && hour < 12)
//                    hour += 12;
//                if (ampm === 'AM' && hour === 12)
//                    hour = 0;
//            }

//            if (isLeft) {
//                var start = this.startDate.clone();
//                start.hour(hour);
//                start.minute(minute);
//                start.second(second);
//                this.setStartDate(start);
//                if (this.singleDatePicker) {
//                    this.endDate = this.startDate.clone();
//                } else if (this.endDate && this.endDate.format('YYYY-MM-DD') == start.format('YYYY-MM-DD') && this.endDate.isBefore(start)) {
//                    this.setEndDate(start.clone());
//                }
//            } else if (this.endDate) {
//                var end = this.endDate.clone();
//                end.hour(hour);
//                end.minute(minute);
//                end.second(second);
//                this.setEndDate(end);
//            }

//            //update the calendars so all clickable dates reflect the new time component
//            this.updateCalendars();

//            //update the form inputs above the calendars with the new time
//            this.updateFormInputs();

//            //re-render the time pickers because changing one selection can affect what's enabled in another
//            this.renderTimePicker('left');
//            this.renderTimePicker('right');

//        },

//        formInputsChanged: function(e) {
//            var isRight = $(e.target).closest('.calendar').hasClass('right');
//            var start = moment(this.container.find('input[name="daterangepicker_start"]').val(), this.locale.format);
//            var end = moment(this.container.find('input[name="daterangepicker_end"]').val(), this.locale.format);

//            if (start.isValid() && end.isValid()) {

//                if (isRight && end.isBefore(start))
//                    start = end.clone();

//                this.setStartDate(start);
//                this.setEndDate(end);

//                if (isRight) {
//                    this.container.find('input[name="daterangepicker_start"]').val(this.startDate.format(this.locale.format));
//                } else {
//                    this.container.find('input[name="daterangepicker_end"]').val(this.endDate.format(this.locale.format));
//                }

//            }

//            this.updateView();
//        },

//        formInputsFocused: function(e) {

//            // Highlight the focused input
//            this.container.find('input[name="daterangepicker_start"], input[name="daterangepicker_end"]').removeClass('active');
//            $(e.target).addClass('active');

//            // Set the state such that if the user goes back to using a mouse, 
//            // the calendars are aware we're selecting the end of the range, not
//            // the start. This allows someone to edit the end of a date range without
//            // re-selecting the beginning, by clicking on the end date input then
//            // using the calendar.
//            var isRight = $(e.target).closest('.calendar').hasClass('right');
//            if (isRight) {
//                this.endDate = null;
//                this.setStartDate(this.startDate.clone());
//                this.updateView();
//            }

//        },

//        formInputsBlurred: function(e) {

//            // this function has one purpose right now: if you tab from the first
//            // text input to the second in the UI, the endDate is nulled so that
//            // you can click another, but if you tab out without clicking anything
//            // or changing the input value, the old endDate should be retained

//            if (!this.endDate) {
//                var val = this.container.find('input[name="daterangepicker_end"]').val();
//                var end = moment(val, this.locale.format);
//                if (end.isValid()) {
//                    this.setEndDate(end);
//                    this.updateView();
//                }
//            }

//        },

//        elementChanged: function() {
//            if (!this.element.is('input')) return;
//            if (!this.element.val().length) return;
//            if (this.element.val().length < this.locale.format.length) return;

//            var dateString = this.element.val().split(this.locale.separator),
//                start = null,
//                end = null;

//            if (dateString.length === 2) {
//                start = moment(dateString[0], this.locale.format);
//                end = moment(dateString[1], this.locale.format);
//            }

//            if (this.singleDatePicker || start === null || end === null) {
//                start = moment(this.element.val(), this.locale.format);
//                end = start;
//            }

//            if (!start.isValid() || !end.isValid()) return;

//            this.setStartDate(start);
//            this.setEndDate(end);
//            this.updateView();
//        },

//        keydown: function(e) {
//            //hide on tab or enter
//            if ((e.keyCode === 9) || (e.keyCode === 13)) {
//                this.hide();
//            }
//        },

//        updateElement: function() {
//            if (this.element.is('input') && !this.singleDatePicker && this.autoUpdateInput) {
//                this.element.val(this.startDate.format(this.locale.format) + this.locale.separator + this.endDate.format(this.locale.format));
//                this.element.trigger('change');
//            } else if (this.element.is('input') && this.autoUpdateInput) {
//                this.element.val(this.startDate.format(this.locale.format));
//                this.element.trigger('change');
//            }
//        },

//        remove: function() {
//            this.container.remove();
//            this.element.off('.daterangepicker');
//            this.element.removeData();
//        }

//    };

//    $.fn.daterangepicker = function(options, callback) {
//        this.each(function() {
//            var el = $(this);
//            if (el.data('daterangepicker'))
//                el.data('daterangepicker').remove();
//            el.data('daterangepicker', new DateRangePicker(el, options, callback));
//        });
//        return this;
//    };

//    return DateRangePicker;

//}));

/**
 * Minified by jsDelivr using Terser v3.14.1.
 * Original file: /npm/daterangepicker@3.0.5/daterangepicker.js
 * 
 * Do NOT use SRI with dynamically generated files! More information: https://www.jsdelivr.com/using-sri-with-dynamic-files
 */
!function (t, e) { if ("function" == typeof define && define.amd) define(["moment", "jquery"], function (t, a) { return a.fn || (a.fn = {}), "function" != typeof t && t.default && (t = t.default), e(t, a) }); else if ("object" == typeof module && module.exports) { var a = "undefined" != typeof window ? window.jQuery : void 0; a || (a = require("jquery")).fn || (a.fn = {}); var i = "undefined" != typeof window && void 0 !== window.moment ? window.moment : require("moment"); module.exports = e(i, a) } else t.daterangepicker = e(t.moment, t.jQuery) }(this, function (t, e) { var a = function (a, i, s) { if (this.parentEl = "body", this.element = e(a), this.startDate = t().startOf("day"), this.endDate = t().endOf("day"), this.minDate = !1, this.maxDate = !1, this.maxSpan = !1, this.autoApply = !1, this.singleDatePicker = !1, this.showDropdowns = !1, this.minYear = t().subtract(100, "year").format("YYYY"), this.maxYear = t().add(100, "year").format("YYYY"), this.showWeekNumbers = !1, this.showISOWeekNumbers = !1, this.showCustomRangeLabel = !0, this.timePicker = !1, this.timePicker24Hour = !1, this.timePickerIncrement = 1, this.timePickerSeconds = !1, this.linkedCalendars = !0, this.autoUpdateInput = !0, this.alwaysShowCalendars = !1, this.ranges = {}, this.opens = "right", this.element.hasClass("pull-right") && (this.opens = "left"), this.drops = "down", this.element.hasClass("dropup") && (this.drops = "up"), this.buttonClasses = "btn btn-sm", this.applyButtonClasses = "btn-primary", this.cancelButtonClasses = "btn-default", this.locale = { direction: "ltr", format: t.localeData().longDateFormat("L"), separator: " - ", applyLabel: "Apply", cancelLabel: "Cancel", weekLabel: "W", customRangeLabel: "Custom Range", daysOfWeek: t.weekdaysMin(), monthNames: t.monthsShort(), firstDay: t.localeData().firstDayOfWeek() }, this.callback = function () { }, this.isShowing = !1, this.leftCalendar = {}, this.rightCalendar = {}, "object" == typeof i && null !== i || (i = {}), "string" == typeof (i = e.extend(this.element.data(), i)).template || i.template instanceof e || (i.template = '<div class="daterangepicker"><div class="ranges"></div><div class="drp-calendar left"><div class="calendar-table"></div><div class="calendar-time"></div></div><div class="drp-calendar right"><div class="calendar-table"></div><div class="calendar-time"></div></div><div class="drp-buttons"><span class="drp-selected"></span><button class="cancelBtn" type="button"></button><button class="applyBtn" disabled="disabled" type="button"></button> </div></div>'), this.parentEl = i.parentEl && e(i.parentEl).length ? e(i.parentEl) : e(this.parentEl), this.container = e(i.template).appendTo(this.parentEl), "object" == typeof i.locale && ("string" == typeof i.locale.direction && (this.locale.direction = i.locale.direction), "string" == typeof i.locale.format && (this.locale.format = i.locale.format), "string" == typeof i.locale.separator && (this.locale.separator = i.locale.separator), "object" == typeof i.locale.daysOfWeek && (this.locale.daysOfWeek = i.locale.daysOfWeek.slice()), "object" == typeof i.locale.monthNames && (this.locale.monthNames = i.locale.monthNames.slice()), "number" == typeof i.locale.firstDay && (this.locale.firstDay = i.locale.firstDay), "string" == typeof i.locale.applyLabel && (this.locale.applyLabel = i.locale.applyLabel), "string" == typeof i.locale.cancelLabel && (this.locale.cancelLabel = i.locale.cancelLabel), "string" == typeof i.locale.weekLabel && (this.locale.weekLabel = i.locale.weekLabel), "string" == typeof i.locale.customRangeLabel)) { (f = document.createElement("textarea")).innerHTML = i.locale.customRangeLabel; var n = f.value; this.locale.customRangeLabel = n } if (this.container.addClass(this.locale.direction), "string" == typeof i.startDate && (this.startDate = t(i.startDate, this.locale.format)), "string" == typeof i.endDate && (this.endDate = t(i.endDate, this.locale.format)), "string" == typeof i.minDate && (this.minDate = t(i.minDate, this.locale.format)), "string" == typeof i.maxDate && (this.maxDate = t(i.maxDate, this.locale.format)), "object" == typeof i.startDate && (this.startDate = t(i.startDate)), "object" == typeof i.endDate && (this.endDate = t(i.endDate)), "object" == typeof i.minDate && (this.minDate = t(i.minDate)), "object" == typeof i.maxDate && (this.maxDate = t(i.maxDate)), this.minDate && this.startDate.isBefore(this.minDate) && (this.startDate = this.minDate.clone()), this.maxDate && this.endDate.isAfter(this.maxDate) && (this.endDate = this.maxDate.clone()), "string" == typeof i.applyButtonClasses && (this.applyButtonClasses = i.applyButtonClasses), "string" == typeof i.applyClass && (this.applyButtonClasses = i.applyClass), "string" == typeof i.cancelButtonClasses && (this.cancelButtonClasses = i.cancelButtonClasses), "string" == typeof i.cancelClass && (this.cancelButtonClasses = i.cancelClass), "object" == typeof i.maxSpan && (this.maxSpan = i.maxSpan), "object" == typeof i.dateLimit && (this.maxSpan = i.dateLimit), "string" == typeof i.opens && (this.opens = i.opens), "string" == typeof i.drops && (this.drops = i.drops), "boolean" == typeof i.showWeekNumbers && (this.showWeekNumbers = i.showWeekNumbers), "boolean" == typeof i.showISOWeekNumbers && (this.showISOWeekNumbers = i.showISOWeekNumbers), "string" == typeof i.buttonClasses && (this.buttonClasses = i.buttonClasses), "object" == typeof i.buttonClasses && (this.buttonClasses = i.buttonClasses.join(" ")), "boolean" == typeof i.showDropdowns && (this.showDropdowns = i.showDropdowns), "number" == typeof i.minYear && (this.minYear = i.minYear), "number" == typeof i.maxYear && (this.maxYear = i.maxYear), "boolean" == typeof i.showCustomRangeLabel && (this.showCustomRangeLabel = i.showCustomRangeLabel), "boolean" == typeof i.singleDatePicker && (this.singleDatePicker = i.singleDatePicker, this.singleDatePicker && (this.endDate = this.startDate.clone())), "boolean" == typeof i.timePicker && (this.timePicker = i.timePicker), "boolean" == typeof i.timePickerSeconds && (this.timePickerSeconds = i.timePickerSeconds), "number" == typeof i.timePickerIncrement && (this.timePickerIncrement = i.timePickerIncrement), "boolean" == typeof i.timePicker24Hour && (this.timePicker24Hour = i.timePicker24Hour), "boolean" == typeof i.autoApply && (this.autoApply = i.autoApply), "boolean" == typeof i.autoUpdateInput && (this.autoUpdateInput = i.autoUpdateInput), "boolean" == typeof i.linkedCalendars && (this.linkedCalendars = i.linkedCalendars), "function" == typeof i.isInvalidDate && (this.isInvalidDate = i.isInvalidDate), "function" == typeof i.isCustomDate && (this.isCustomDate = i.isCustomDate), "boolean" == typeof i.alwaysShowCalendars && (this.alwaysShowCalendars = i.alwaysShowCalendars), 0 != this.locale.firstDay) for (var r = this.locale.firstDay; r > 0;)this.locale.daysOfWeek.push(this.locale.daysOfWeek.shift()), r--; var o, l, h; if (void 0 === i.startDate && void 0 === i.endDate && e(this.element).is(":text")) { var c = e(this.element).val(), d = c.split(this.locale.separator); o = l = null, 2 == d.length ? (o = t(d[0], this.locale.format), l = t(d[1], this.locale.format)) : this.singleDatePicker && "" !== c && (o = t(c, this.locale.format), l = t(c, this.locale.format)), null !== o && null !== l && (this.setStartDate(o), this.setEndDate(l)) } if ("object" == typeof i.ranges) { for (h in i.ranges) { o = "string" == typeof i.ranges[h][0] ? t(i.ranges[h][0], this.locale.format) : t(i.ranges[h][0]), l = "string" == typeof i.ranges[h][1] ? t(i.ranges[h][1], this.locale.format) : t(i.ranges[h][1]), this.minDate && o.isBefore(this.minDate) && (o = this.minDate.clone()); var m = this.maxDate; if (this.maxSpan && m && o.clone().add(this.maxSpan).isAfter(m) && (m = o.clone().add(this.maxSpan)), m && l.isAfter(m) && (l = m.clone()), !(this.minDate && l.isBefore(this.minDate, this.timepicker ? "minute" : "day") || m && o.isAfter(m, this.timepicker ? "minute" : "day"))) { var f; (f = document.createElement("textarea")).innerHTML = h; n = f.value; this.ranges[n] = [o, l] } } var p = "<ul>"; for (h in this.ranges) p += '<li data-range-key="' + h + '">' + h + "</li>"; this.showCustomRangeLabel && (p += '<li data-range-key="' + this.locale.customRangeLabel + '">' + this.locale.customRangeLabel + "</li>"), p += "</ul>", this.container.find(".ranges").prepend(p) } "function" == typeof s && (this.callback = s), this.timePicker || (this.startDate = this.startDate.startOf("day"), this.endDate = this.endDate.endOf("day"), this.container.find(".calendar-time").hide()), this.timePicker && this.autoApply && (this.autoApply = !1), this.autoApply && this.container.addClass("auto-apply"), "object" == typeof i.ranges && this.container.addClass("show-ranges"), this.singleDatePicker && (this.container.addClass("single"), this.container.find(".drp-calendar.left").addClass("single"), this.container.find(".drp-calendar.left").show(), this.container.find(".drp-calendar.right").hide(), this.timePicker || this.container.addClass("auto-apply")), (void 0 === i.ranges && !this.singleDatePicker || this.alwaysShowCalendars) && this.container.addClass("show-calendar"), this.container.addClass("opens" + this.opens), this.container.find(".applyBtn, .cancelBtn").addClass(this.buttonClasses), this.applyButtonClasses.length && this.container.find(".applyBtn").addClass(this.applyButtonClasses), this.cancelButtonClasses.length && this.container.find(".cancelBtn").addClass(this.cancelButtonClasses), this.container.find(".applyBtn").html(this.locale.applyLabel), this.container.find(".cancelBtn").html(this.locale.cancelLabel), this.container.find(".drp-calendar").on("click.daterangepicker", ".prev", e.proxy(this.clickPrev, this)).on("click.daterangepicker", ".next", e.proxy(this.clickNext, this)).on("mousedown.daterangepicker", "td.available", e.proxy(this.clickDate, this)).on("mouseenter.daterangepicker", "td.available", e.proxy(this.hoverDate, this)).on("change.daterangepicker", "select.yearselect", e.proxy(this.monthOrYearChanged, this)).on("change.daterangepicker", "select.monthselect", e.proxy(this.monthOrYearChanged, this)).on("change.daterangepicker", "select.hourselect,select.minuteselect,select.secondselect,select.ampmselect", e.proxy(this.timeChanged, this)), this.container.find(".ranges").on("click.daterangepicker", "li", e.proxy(this.clickRange, this)), this.container.find(".drp-buttons").on("click.daterangepicker", "button.applyBtn", e.proxy(this.clickApply, this)).on("click.daterangepicker", "button.cancelBtn", e.proxy(this.clickCancel, this)), this.element.is("input") || this.element.is("button") ? this.element.on({ "click.daterangepicker": e.proxy(this.show, this), "focus.daterangepicker": e.proxy(this.show, this), "keyup.daterangepicker": e.proxy(this.elementChanged, this), "keydown.daterangepicker": e.proxy(this.keydown, this) }) : (this.element.on("click.daterangepicker", e.proxy(this.toggle, this)), this.element.on("keydown.daterangepicker", e.proxy(this.toggle, this))), this.updateElement() }; return a.prototype = { constructor: a, setStartDate: function (e) { "string" == typeof e && (this.startDate = t(e, this.locale.format)), "object" == typeof e && (this.startDate = t(e)), this.timePicker || (this.startDate = this.startDate.startOf("day")), this.timePicker && this.timePickerIncrement && this.startDate.minute(Math.round(this.startDate.minute() / this.timePickerIncrement) * this.timePickerIncrement), this.minDate && this.startDate.isBefore(this.minDate) && (this.startDate = this.minDate.clone(), this.timePicker && this.timePickerIncrement && this.startDate.minute(Math.round(this.startDate.minute() / this.timePickerIncrement) * this.timePickerIncrement)), this.maxDate && this.startDate.isAfter(this.maxDate) && (this.startDate = this.maxDate.clone(), this.timePicker && this.timePickerIncrement && this.startDate.minute(Math.floor(this.startDate.minute() / this.timePickerIncrement) * this.timePickerIncrement)), this.isShowing || this.updateElement(), this.updateMonthsInView() }, setEndDate: function (e) { "string" == typeof e && (this.endDate = t(e, this.locale.format)), "object" == typeof e && (this.endDate = t(e)), this.timePicker || (this.endDate = this.endDate.endOf("day")), this.timePicker && this.timePickerIncrement && this.endDate.minute(Math.round(this.endDate.minute() / this.timePickerIncrement) * this.timePickerIncrement), this.endDate.isBefore(this.startDate) && (this.endDate = this.startDate.clone()), this.maxDate && this.endDate.isAfter(this.maxDate) && (this.endDate = this.maxDate.clone()), this.maxSpan && this.startDate.clone().add(this.maxSpan).isBefore(this.endDate) && (this.endDate = this.startDate.clone().add(this.maxSpan)), this.previousRightTime = this.endDate.clone(), this.container.find(".drp-selected").html(this.startDate.format(this.locale.format) + this.locale.separator + this.endDate.format(this.locale.format)), this.isShowing || this.updateElement(), this.updateMonthsInView() }, isInvalidDate: function () { return !1 }, isCustomDate: function () { return !1 }, updateView: function () { this.timePicker && (this.renderTimePicker("left"), this.renderTimePicker("right"), this.endDate ? this.container.find(".right .calendar-time select").removeAttr("disabled").removeClass("disabled") : this.container.find(".right .calendar-time select").attr("disabled", "disabled").addClass("disabled")), this.endDate && this.container.find(".drp-selected").html(this.startDate.format(this.locale.format) + this.locale.separator + this.endDate.format(this.locale.format)), this.updateMonthsInView(), this.updateCalendars(), this.updateFormInputs() }, updateMonthsInView: function () { if (this.endDate) { if (!this.singleDatePicker && this.leftCalendar.month && this.rightCalendar.month && (this.startDate.format("YYYY-MM") == this.leftCalendar.month.format("YYYY-MM") || this.startDate.format("YYYY-MM") == this.rightCalendar.month.format("YYYY-MM")) && (this.endDate.format("YYYY-MM") == this.leftCalendar.month.format("YYYY-MM") || this.endDate.format("YYYY-MM") == this.rightCalendar.month.format("YYYY-MM"))) return; this.leftCalendar.month = this.startDate.clone().date(2), this.linkedCalendars || this.endDate.month() == this.startDate.month() && this.endDate.year() == this.startDate.year() ? this.rightCalendar.month = this.startDate.clone().date(2).add(1, "month") : this.rightCalendar.month = this.endDate.clone().date(2) } else this.leftCalendar.month.format("YYYY-MM") != this.startDate.format("YYYY-MM") && this.rightCalendar.month.format("YYYY-MM") != this.startDate.format("YYYY-MM") && (this.leftCalendar.month = this.startDate.clone().date(2), this.rightCalendar.month = this.startDate.clone().date(2).add(1, "month")); this.maxDate && this.linkedCalendars && !this.singleDatePicker && this.rightCalendar.month > this.maxDate && (this.rightCalendar.month = this.maxDate.clone().date(2), this.leftCalendar.month = this.maxDate.clone().date(2).subtract(1, "month")) }, updateCalendars: function () { if (this.timePicker) { var t, e, a, i; if (this.endDate) { if (t = parseInt(this.container.find(".left .hourselect").val(), 10), e = parseInt(this.container.find(".left .minuteselect").val(), 10), isNaN(e) && (e = parseInt(this.container.find(".left .minuteselect option:last").val(), 10)), a = this.timePickerSeconds ? parseInt(this.container.find(".left .secondselect").val(), 10) : 0, !this.timePicker24Hour) "PM" === (i = this.container.find(".left .ampmselect").val()) && t < 12 && (t += 12), "AM" === i && 12 === t && (t = 0) } else if (t = parseInt(this.container.find(".right .hourselect").val(), 10), e = parseInt(this.container.find(".right .minuteselect").val(), 10), isNaN(e) && (e = parseInt(this.container.find(".right .minuteselect option:last").val(), 10)), a = this.timePickerSeconds ? parseInt(this.container.find(".right .secondselect").val(), 10) : 0, !this.timePicker24Hour) "PM" === (i = this.container.find(".right .ampmselect").val()) && t < 12 && (t += 12), "AM" === i && 12 === t && (t = 0); this.leftCalendar.month.hour(t).minute(e).second(a), this.rightCalendar.month.hour(t).minute(e).second(a) } this.renderCalendar("left"), this.renderCalendar("right"), this.container.find(".ranges li").removeClass("active"), null != this.endDate && this.calculateChosenLabel() }, renderCalendar: function (a) { var i, s = (i = "left" == a ? this.leftCalendar : this.rightCalendar).month.month(), n = i.month.year(), r = i.month.hour(), o = i.month.minute(), l = i.month.second(), h = t([n, s]).daysInMonth(), c = t([n, s, 1]), d = t([n, s, h]), m = t(c).subtract(1, "month").month(), f = t(c).subtract(1, "month").year(), p = t([f, m]).daysInMonth(), u = c.day(); (i = []).firstDay = c, i.lastDay = d; for (var D = 0; D < 6; D++)i[D] = []; var g = p - u + this.locale.firstDay + 1; g > p && (g -= 7), u == this.locale.firstDay && (g = p - 6); for (var y = t([f, m, g, 12, o, l]), k = (D = 0, 0), b = 0; D < 42; D++ , k++ , y = t(y).add(24, "hour"))D > 0 && k % 7 == 0 && (k = 0, b++), i[b][k] = y.clone().hour(r).minute(o).second(l), y.hour(12), this.minDate && i[b][k].format("YYYY-MM-DD") == this.minDate.format("YYYY-MM-DD") && i[b][k].isBefore(this.minDate) && "left" == a && (i[b][k] = this.minDate.clone()), this.maxDate && i[b][k].format("YYYY-MM-DD") == this.maxDate.format("YYYY-MM-DD") && i[b][k].isAfter(this.maxDate) && "right" == a && (i[b][k] = this.maxDate.clone()); "left" == a ? this.leftCalendar.calendar = i : this.rightCalendar.calendar = i; var v = "left" == a ? this.minDate : this.startDate, C = this.maxDate, Y = ("left" == a ? this.startDate : this.endDate, this.locale.direction, '<table class="table-condensed">'); Y += "<thead>", Y += "<tr>", (this.showWeekNumbers || this.showISOWeekNumbers) && (Y += "<th></th>"), v && !v.isBefore(i.firstDay) || this.linkedCalendars && "left" != a ? Y += "<th></th>" : Y += '<th class="prev available"><span></span></th>'; var w = this.locale.monthNames[i[1][1].month()] + i[1][1].format(" YYYY"); if (this.showDropdowns) { for (var P = i[1][1].month(), x = i[1][1].year(), M = C && C.year() || this.maxYear, I = v && v.year() || this.minYear, S = x == I, B = x == M, A = '<select class="monthselect">', L = 0; L < 12; L++)(!S || v && L >= v.month()) && (!B || C && L <= C.month()) ? A += "<option value='" + L + "'" + (L === P ? " selected='selected'" : "") + ">" + this.locale.monthNames[L] + "</option>" : A += "<option value='" + L + "'" + (L === P ? " selected='selected'" : "") + " disabled='disabled'>" + this.locale.monthNames[L] + "</option>"; A += "</select>"; for (var N = '<select class="yearselect">', E = I; E <= M; E++)N += '<option value="' + E + '"' + (E === x ? ' selected="selected"' : "") + ">" + E + "</option>"; w = A + (N += "</select>") } if (Y += '<th colspan="5" class="month">' + w + "</th>", C && !C.isAfter(i.lastDay) || this.linkedCalendars && "right" != a && !this.singleDatePicker ? Y += "<th></th>" : Y += '<th class="next available"><span></span></th>', Y += "</tr>", Y += "<tr>", (this.showWeekNumbers || this.showISOWeekNumbers) && (Y += '<th class="week">' + this.locale.weekLabel + "</th>"), e.each(this.locale.daysOfWeek, function (t, e) { Y += "<th>" + e + "</th>" }), Y += "</tr>", Y += "</thead>", Y += "<tbody>", null == this.endDate && this.maxSpan) { var W = this.startDate.clone().add(this.maxSpan).endOf("day"); C && !W.isBefore(C) || (C = W) } for (b = 0; b < 6; b++) { Y += "<tr>", this.showWeekNumbers ? Y += '<td class="week">' + i[b][0].week() + "</td>" : this.showISOWeekNumbers && (Y += '<td class="week">' + i[b][0].isoWeek() + "</td>"); for (k = 0; k < 7; k++) { var O = []; i[b][k].isSame(new Date, "day") && O.push("today"), i[b][k].isoWeekday() > 5 && O.push("weekend"), i[b][k].month() != i[1][1].month() && O.push("off", "ends"), this.minDate && i[b][k].isBefore(this.minDate, "day") && O.push("off", "disabled"), C && i[b][k].isAfter(C, "day") && O.push("off", "disabled"), this.isInvalidDate(i[b][k]) && O.push("off", "disabled"), i[b][k].format("YYYY-MM-DD") == this.startDate.format("YYYY-MM-DD") && O.push("active", "start-date"), null != this.endDate && i[b][k].format("YYYY-MM-DD") == this.endDate.format("YYYY-MM-DD") && O.push("active", "end-date"), null != this.endDate && i[b][k] > this.startDate && i[b][k] < this.endDate && O.push("in-range"); var H = this.isCustomDate(i[b][k]); !1 !== H && ("string" == typeof H ? O.push(H) : Array.prototype.push.apply(O, H)); var j = "", R = !1; for (D = 0; D < O.length; D++)j += O[D] + " ", "disabled" == O[D] && (R = !0); R || (j += "available"), Y += '<td class="' + j.replace(/^\s+|\s+$/g, "") + '" data-title="r' + b + "c" + k + '">' + i[b][k].date() + "</td>" } Y += "</tr>" } Y += "</tbody>", Y += "</table>", this.container.find(".drp-calendar." + a + " .calendar-table").html(Y) }, renderTimePicker: function (t) { if ("right" != t || this.endDate) { var e, a, i, s = this.maxDate; if (!this.maxSpan || this.maxDate && !this.startDate.clone().add(this.maxSpan).isBefore(this.maxDate) || (s = this.startDate.clone().add(this.maxSpan)), "left" == t) a = this.startDate.clone(), i = this.minDate; else if ("right" == t) { a = this.endDate.clone(), i = this.startDate; var n = this.container.find(".drp-calendar.right .calendar-time"); if ("" != n.html() && (a.hour(isNaN(a.hour()) ? n.find(".hourselect option:selected").val() : a.hour()), a.minute(isNaN(a.minute()) ? n.find(".minuteselect option:selected").val() : a.minute()), a.second(isNaN(a.second()) ? n.find(".secondselect option:selected").val() : a.second()), !this.timePicker24Hour)) { var r = n.find(".ampmselect option:selected").val(); "PM" === r && a.hour() < 12 && a.hour(a.hour() + 12), "AM" === r && 12 === a.hour() && a.hour(0) } a.isBefore(this.startDate) && (a = this.startDate.clone()), s && a.isAfter(s) && (a = s.clone()) } e = '<select class="hourselect">'; for (var o = this.timePicker24Hour ? 0 : 1, l = this.timePicker24Hour ? 23 : 12, h = o; h <= l; h++) { var c = h; this.timePicker24Hour || (c = a.hour() >= 12 ? 12 == h ? 12 : h + 12 : 12 == h ? 0 : h); var d = a.clone().hour(c), m = !1; i && d.minute(59).isBefore(i) && (m = !0), s && d.minute(0).isAfter(s) && (m = !0), c != a.hour() || m ? e += m ? '<option value="' + h + '" disabled="disabled" class="disabled">' + h + "</option>" : '<option value="' + h + '">' + h + "</option>" : e += '<option value="' + h + '" selected="selected">' + h + "</option>" } e += "</select> ", e += ': <select class="minuteselect">'; for (h = 0; h < 60; h += this.timePickerIncrement) { var f = h < 10 ? "0" + h : h; d = a.clone().minute(h), m = !1; i && d.second(59).isBefore(i) && (m = !0), s && d.second(0).isAfter(s) && (m = !0), a.minute() != h || m ? e += m ? '<option value="' + h + '" disabled="disabled" class="disabled">' + f + "</option>" : '<option value="' + h + '">' + f + "</option>" : e += '<option value="' + h + '" selected="selected">' + f + "</option>" } if (e += "</select> ", this.timePickerSeconds) { e += ': <select class="secondselect">'; for (h = 0; h < 60; h++) { f = h < 10 ? "0" + h : h, d = a.clone().second(h), m = !1; i && d.isBefore(i) && (m = !0), s && d.isAfter(s) && (m = !0), a.second() != h || m ? e += m ? '<option value="' + h + '" disabled="disabled" class="disabled">' + f + "</option>" : '<option value="' + h + '">' + f + "</option>" : e += '<option value="' + h + '" selected="selected">' + f + "</option>" } e += "</select> " } if (!this.timePicker24Hour) { e += '<select class="ampmselect">'; var p = "", u = ""; i && a.clone().hour(12).minute(0).second(0).isBefore(i) && (p = ' disabled="disabled" class="disabled"'), s && a.clone().hour(0).minute(0).second(0).isAfter(s) && (u = ' disabled="disabled" class="disabled"'), a.hour() >= 12 ? e += '<option value="AM"' + p + '>AM</option><option value="PM" selected="selected"' + u + ">PM</option>" : e += '<option value="AM" selected="selected"' + p + '>AM</option><option value="PM"' + u + ">PM</option>", e += "</select>" } this.container.find(".drp-calendar." + t + " .calendar-time").html(e) } }, updateFormInputs: function () { this.singleDatePicker || this.endDate && (this.startDate.isBefore(this.endDate) || this.startDate.isSame(this.endDate)) ? this.container.find("button.applyBtn").removeAttr("disabled") : this.container.find("button.applyBtn").attr("disabled", "disabled") }, move: function () { var t, a = { top: 0, left: 0 }, i = e(window).width(); this.parentEl.is("body") || (a = { top: this.parentEl.offset().top - this.parentEl.scrollTop(), left: this.parentEl.offset().left - this.parentEl.scrollLeft() }, i = this.parentEl[0].clientWidth + this.parentEl.offset().left), t = "up" == this.drops ? this.element.offset().top - this.container.outerHeight() - a.top : this.element.offset().top + this.element.outerHeight() - a.top, this.container.css({ top: 0, left: 0, right: "auto" }); var s = this.container.outerWidth(); if (this.container["up" == this.drops ? "addClass" : "removeClass"]("drop-up"), "left" == this.opens) { var n = i - this.element.offset().left - this.element.outerWidth(); s + n > e(window).width() ? this.container.css({ top: t, right: "auto", left: 9 }) : this.container.css({ top: t, right: n, left: "auto" }) } else if ("center" == this.opens) { (r = this.element.offset().left - a.left + this.element.outerWidth() / 2 - s / 2) < 0 ? this.container.css({ top: t, right: "auto", left: 9 }) : r + s > e(window).width() ? this.container.css({ top: t, left: "auto", right: 0 }) : this.container.css({ top: t, left: r, right: "auto" }) } else { var r; (r = this.element.offset().left - a.left) + s > e(window).width() ? this.container.css({ top: t, left: "auto", right: 0 }) : this.container.css({ top: t, left: r, right: "auto" }) } }, show: function (t) { this.isShowing || (this._outsideClickProxy = e.proxy(function (t) { this.outsideClick(t) }, this), e(document).on("mousedown.daterangepicker", this._outsideClickProxy).on("touchend.daterangepicker", this._outsideClickProxy).on("click.daterangepicker", "[data-toggle=dropdown]", this._outsideClickProxy).on("focusin.daterangepicker", this._outsideClickProxy), e(window).on("resize.daterangepicker", e.proxy(function (t) { this.move(t) }, this)), this.oldStartDate = this.startDate.clone(), this.oldEndDate = this.endDate.clone(), this.previousRightTime = this.endDate.clone(), this.updateView(), this.container.show(), this.move(), this.element.trigger("show.daterangepicker", this), this.isShowing = !0) }, hide: function (t) { this.isShowing && (this.endDate || (this.startDate = this.oldStartDate.clone(), this.endDate = this.oldEndDate.clone()), this.startDate.isSame(this.oldStartDate) && this.endDate.isSame(this.oldEndDate) || this.callback(this.startDate.clone(), this.endDate.clone(), this.chosenLabel), this.updateElement(), e(document).off(".daterangepicker"), e(window).off(".daterangepicker"), this.container.hide(), this.element.trigger("hide.daterangepicker", this), this.isShowing = !1) }, toggle: function (t) { this.isShowing ? this.hide() : this.show() }, outsideClick: function (t) { var a = e(t.target); "focusin" == t.type || a.closest(this.element).length || a.closest(this.container).length || a.closest(".calendar-table").length || (this.hide(), this.element.trigger("outsideClick.daterangepicker", this)) }, showCalendars: function () { this.container.addClass("show-calendar"), this.move(), this.element.trigger("showCalendar.daterangepicker", this) }, hideCalendars: function () { this.container.removeClass("show-calendar"), this.element.trigger("hideCalendar.daterangepicker", this) }, clickRange: function (t) { var e = t.target.getAttribute("data-range-key"); if (this.chosenLabel = e, e == this.locale.customRangeLabel) this.showCalendars(); else { var a = this.ranges[e]; this.startDate = a[0], this.endDate = a[1], this.timePicker || (this.startDate.startOf("day"), this.endDate.endOf("day")), this.alwaysShowCalendars || this.hideCalendars(), this.clickApply() } }, clickPrev: function (t) { e(t.target).parents(".drp-calendar").hasClass("left") ? (this.leftCalendar.month.subtract(1, "month"), this.linkedCalendars && this.rightCalendar.month.subtract(1, "month")) : this.rightCalendar.month.subtract(1, "month"), this.updateCalendars() }, clickNext: function (t) { e(t.target).parents(".drp-calendar").hasClass("left") ? this.leftCalendar.month.add(1, "month") : (this.rightCalendar.month.add(1, "month"), this.linkedCalendars && this.leftCalendar.month.add(1, "month")), this.updateCalendars() }, hoverDate: function (t) { if (e(t.target).hasClass("available")) { var a = e(t.target).attr("data-title"), i = a.substr(1, 1), s = a.substr(3, 1), n = e(t.target).parents(".drp-calendar").hasClass("left") ? this.leftCalendar.calendar[i][s] : this.rightCalendar.calendar[i][s], r = this.leftCalendar, o = this.rightCalendar, l = this.startDate; this.endDate || this.container.find(".drp-calendar tbody td").each(function (t, a) { if (!e(a).hasClass("week")) { var i = e(a).attr("data-title"), s = i.substr(1, 1), h = i.substr(3, 1), c = e(a).parents(".drp-calendar").hasClass("left") ? r.calendar[s][h] : o.calendar[s][h]; c.isAfter(l) && c.isBefore(n) || c.isSame(n, "day") ? e(a).addClass("in-range") : e(a).removeClass("in-range") } }) } }, clickDate: function (t) { if (e(t.target).hasClass("available")) { var a = e(t.target).attr("data-title"), i = a.substr(1, 1), s = a.substr(3, 1), n = e(t.target).parents(".drp-calendar").hasClass("left") ? this.leftCalendar.calendar[i][s] : this.rightCalendar.calendar[i][s]; if (this.endDate || n.isBefore(this.startDate, "day")) { if (this.timePicker) { var r = parseInt(this.container.find(".left .hourselect").val(), 10); if (!this.timePicker24Hour) "PM" === (h = this.container.find(".left .ampmselect").val()) && r < 12 && (r += 12), "AM" === h && 12 === r && (r = 0); var o = parseInt(this.container.find(".left .minuteselect").val(), 10); isNaN(o) && (o = parseInt(this.container.find(".left .minuteselect option:last").val(), 10)); var l = this.timePickerSeconds ? parseInt(this.container.find(".left .secondselect").val(), 10) : 0; n = n.clone().hour(r).minute(o).second(l) } this.endDate = null, this.setStartDate(n.clone()) } else if (!this.endDate && n.isBefore(this.startDate)) this.setEndDate(this.startDate.clone()); else { if (this.timePicker) { var h; r = parseInt(this.container.find(".right .hourselect").val(), 10); if (!this.timePicker24Hour) "PM" === (h = this.container.find(".right .ampmselect").val()) && r < 12 && (r += 12), "AM" === h && 12 === r && (r = 0); o = parseInt(this.container.find(".right .minuteselect").val(), 10); isNaN(o) && (o = parseInt(this.container.find(".right .minuteselect option:last").val(), 10)); l = this.timePickerSeconds ? parseInt(this.container.find(".right .secondselect").val(), 10) : 0; n = n.clone().hour(r).minute(o).second(l) } this.setEndDate(n.clone()), this.autoApply && (this.calculateChosenLabel(), this.clickApply()) } this.singleDatePicker && (this.setEndDate(this.startDate), this.timePicker || this.clickApply()), this.updateView(), t.stopPropagation() } }, calculateChosenLabel: function () { var t = !0, e = 0; for (var a in this.ranges) { if (this.timePicker) { var i = this.timePickerSeconds ? "YYYY-MM-DD HH:mm:ss" : "YYYY-MM-DD HH:mm"; if (this.startDate.format(i) == this.ranges[a][0].format(i) && this.endDate.format(i) == this.ranges[a][1].format(i)) { t = !1, this.chosenLabel = this.container.find(".ranges li:eq(" + e + ")").addClass("active").attr("data-range-key"); break } } else if (this.startDate.format("YYYY-MM-DD") == this.ranges[a][0].format("YYYY-MM-DD") && this.endDate.format("YYYY-MM-DD") == this.ranges[a][1].format("YYYY-MM-DD")) { t = !1, this.chosenLabel = this.container.find(".ranges li:eq(" + e + ")").addClass("active").attr("data-range-key"); break } e++ } t && (this.showCustomRangeLabel ? this.chosenLabel = this.container.find(".ranges li:last").addClass("active").attr("data-range-key") : this.chosenLabel = null, this.showCalendars()) }, clickApply: function (t) { this.hide(), this.element.trigger("apply.daterangepicker", this) }, clickCancel: function (t) { this.startDate = this.oldStartDate, this.endDate = this.oldEndDate, this.hide(), this.element.trigger("cancel.daterangepicker", this) }, monthOrYearChanged: function (t) { var a = e(t.target).closest(".drp-calendar").hasClass("left"), i = a ? "left" : "right", s = this.container.find(".drp-calendar." + i), n = parseInt(s.find(".monthselect").val(), 10), r = s.find(".yearselect").val(); a || (r < this.startDate.year() || r == this.startDate.year() && n < this.startDate.month()) && (n = this.startDate.month(), r = this.startDate.year()), this.minDate && (r < this.minDate.year() || r == this.minDate.year() && n < this.minDate.month()) && (n = this.minDate.month(), r = this.minDate.year()), this.maxDate && (r > this.maxDate.year() || r == this.maxDate.year() && n > this.maxDate.month()) && (n = this.maxDate.month(), r = this.maxDate.year()), a ? (this.leftCalendar.month.month(n).year(r), this.linkedCalendars && (this.rightCalendar.month = this.leftCalendar.month.clone().add(1, "month"))) : (this.rightCalendar.month.month(n).year(r), this.linkedCalendars && (this.leftCalendar.month = this.rightCalendar.month.clone().subtract(1, "month"))), this.updateCalendars() }, timeChanged: function (t) { var a = e(t.target).closest(".drp-calendar"), i = a.hasClass("left"), s = parseInt(a.find(".hourselect").val(), 10), n = parseInt(a.find(".minuteselect").val(), 10); isNaN(n) && (n = parseInt(a.find(".minuteselect option:last").val(), 10)); var r = this.timePickerSeconds ? parseInt(a.find(".secondselect").val(), 10) : 0; if (!this.timePicker24Hour) { var o = a.find(".ampmselect").val(); "PM" === o && s < 12 && (s += 12), "AM" === o && 12 === s && (s = 0) } if (i) { var l = this.startDate.clone(); l.hour(s), l.minute(n), l.second(r), this.setStartDate(l), this.singleDatePicker ? this.endDate = this.startDate.clone() : this.endDate && this.endDate.format("YYYY-MM-DD") == l.format("YYYY-MM-DD") && this.endDate.isBefore(l) && this.setEndDate(l.clone()) } else if (this.endDate) { var h = this.endDate.clone(); h.hour(s), h.minute(n), h.second(r), this.setEndDate(h) } this.updateCalendars(), this.updateFormInputs(), this.renderTimePicker("left"), this.renderTimePicker("right") }, elementChanged: function () { if (this.element.is("input") && this.element.val().length) { var e = this.element.val().split(this.locale.separator), a = null, i = null; 2 === e.length && (a = t(e[0], this.locale.format), i = t(e[1], this.locale.format)), (this.singleDatePicker || null === a || null === i) && (i = a = t(this.element.val(), this.locale.format)), a.isValid() && i.isValid() && (this.setStartDate(a), this.setEndDate(i), this.updateView()) } }, keydown: function (t) { 9 !== t.keyCode && 13 !== t.keyCode || this.hide(), 27 === t.keyCode && (t.preventDefault(), t.stopPropagation(), this.hide()) }, updateElement: function () { if (this.element.is("input") && this.autoUpdateInput) { var t = this.startDate.format(this.locale.format); this.singleDatePicker || (t += this.locale.separator + this.endDate.format(this.locale.format)), t !== this.element.val() && this.element.val(t).trigger("change") } }, remove: function () { this.container.remove(), this.element.off(".daterangepicker"), this.element.removeData() } }, e.fn.daterangepicker = function (t, i) { var s = e.extend(!0, {}, e.fn.daterangepicker.defaultOptions, t); return this.each(function () { var t = e(this); t.data("daterangepicker") && t.data("daterangepicker").remove(), t.data("daterangepicker", new a(t, s, i)) }), this }, a });
//# sourceMappingURL=/sm/3a884fe0bdb97cb3a94b410e67cf38fdc248890d5581232077b3e6241e25cd21.map