// Set default properties of tippy
tippy.setDefaultProps({
    hideOnClick: false, // Do not hide with mouse click
    appendTo: "parent"  // The element to append the tippy to
});

// Add customized validation methods of jQuery validator



// ================================================

// Override original methods (prototype.checkForm) of jQuery Validator

/** 
 *  Check all elements that are fullfill the names in the form respectively
 *  Add a new ignore rule for a customized class "ignore-validate"
 */
$.validator.prototype.checkForm = function () {
    this.prepareForm();
    for (let i = 0, elements = (this.currentElements = this.elements()); elements[i]; i++) {
        if (this.findByName(elements[i].name).length !== undefined && this.findByName(elements[i].name).length > 1) {
            for (let cnt = 0; cnt < this.findByName(elements[i].name).length; cnt++) {
                if (this.findByName(elements[i].name)[cnt].classList.contains("ignore-validate")) {
                    continue;
                }
                let element = this.findByName(elements[i].name)[cnt];
                // Only check the visible elements
                if ($(element).is(":visible")) {
                    this.check(element);
                }
            }
        }
        else {
            this.check(elements[i]);
        }
    }
    return this.valid();
};

// ================================================

/**
 * jQuery Validator operation with tippy
 * @returns
 */
var Validator = (function () {
    // == Parameters ==
    let tippy_id_group_instance_kvp = {};
    let set_placement_to_bottom_types = ["checkbox", 'radio', "date", "time"];
    let tippy_placement_full_list = [
        "top",
        "top-start",
        "top-end",
        "right",
        "right-start",
        "right-end",
        "bottom",
        "bottom-start",
        "bottom-end",
        "left",
        "left-start",
        "left-end",
        "auto",
        "auto-start",
        "auto-end"
    ];

    // == Methods ==
    /**
     * Get appropriate group id for the input $element according to its attribute.
     * This $element must be a jQuery object.
     * @param {Object} $element
     * @returns {string} Group ID of an jQuery object
     */
    let GetGroupIdByjQueryObj = ($element) => {
        let group_id = "";
        let $shadow = $element.closest(".shadow");

        if ($shadow.length === 0) {
            group_id = $element.attr("data-tippy-group") || "ungrouped";
        }
        else {
            group_id = `#${$shadow.attr('id')}`;
        }

        return group_id;
    };

    /**
     * Get appropriate tippy placement value of the input $element according to its attribute.
     * This $element must be a jQuery object.
     * @param {Object} $element
     * @returns {string} tippy placement value of an jQuery object
     */
    let GetPlacementByjQueryObj = ($element) => {
        let type = $element.attr("type");
        let placement = $element.attr("data-tippy-placement") || "";

        if (placement === "" || !tippy_placement_full_list.includes(placement)) {
            if (set_placement_to_bottom_types.includes(type)) {
                placement = "bottom";
            }
            else {
                placement = "right";
            }
        }

        return placement;
    };

    /**
     * Get appropriate tippy trigger value of the input $element according to its attribute.
     * This $element must by a jQuery object.
     * @param {Object} $element
     * @returns {string} tippy trigger value of an jQuery object
     */
    let GetTriggerByjQueryObj = ($element) => {
        let trigger = $element.attr("data-tippy-trigger") || "";

        if (trigger === "") {
            trigger = "manual";
        }

        return trigger;
    };

    // Override original methods (setDefaultes) of jQuery Validator
    $.validator.setDefaults({
        ignoreTitle: true,
        highlight: function (input) {
            $(input).addClass('is-invalid');
        },
        unhighlight: function (input) {
            //$(input).removeClass("is-invalid");
        },
        //submitHandler: function (form) {
        //    return false; // This will reject all submit requests from front end
        //},

        /**
         * Before a field is marked as invalid, the validation is lazy:
         * Before a submitting the form for the first time,
         * the user can tab through fields without getting annoying messages
         * - they won't get bugged before having the chance to actually enter a correct value
         * To fix this issue, we should over-ride the onfocusout default function with a custom one.
         * The parameter "element" is a DOM element
         * @param {string} element
         */
        onfocusout: function (element) {
            let $element = $(element);
            // Processes that have to complete before form checking with jQuery validator can write down here.
            // ...

            this.element(element);
        },

        /**
         * @param {Object} $error
         * @param {Object} $element
         * */
        errorPlacement: function ($error, $element) {
            let error_msg = $error.text();
            if (error_msg.length === 0) {
                return;
            }

            let group_id = GetGroupIdByjQueryObj($element);
            if (tippy_id_group_instance_kvp[group_id] === undefined) {
                tippy_id_group_instance_kvp[group_id] = {};
            }

            let ele_id = `#${$element.attr('id')}`;

            let instance = tippy_id_group_instance_kvp[group_id][ele_id];
            if (instance === undefined) {
                instance = tippy($element[0], {
                    content: error_msg,
                    placement: GetPlacementByjQueryObj($element),
                    trigger: GetTriggerByjQueryObj($element)
                });
                tippy_id_group_instance_kvp[group_id][ele_id] = instance;
            }
            else {
                instance.setContent(error_msg);
            }

            instance.enable();
            instance.show();
        },

        /**
         * The parameters "error" and "element" are DOM elements
         * @param {string} error
         * @param {string} element
         * */
        success: function (error, element) {
            let $element = $(element);
            $element.removeClass('is-invalid');

            let group_id = GetGroupIdByjQueryObj($element);
            if (tippy_id_group_instance_kvp[group_id] === undefined) {
                return;
            }

            let instance = tippy_id_group_instance_kvp[group_id][`#${$element.attr('id')}`];
            if (instance === undefined) {
                return;
            }

            instance.hide();
            instance.disable();
        }
    });

    /**
     * Hide all validation error messages of elements belonged to the input "group_id".
     * The parameter "group_id" is the attribute name of "id" marked with "#".
     * @param {string} group_id
     * */
    let HideTippy = (group_id) => {
        if (tippy_id_group_instance_kvp[group_id] === undefined) {
            return;
        }
        for (let ele_id in tippy_id_group_instance_kvp[group_id]) {
            tippy_id_group_instance_kvp[group_id][ele_id].hide();
        }
    };

    // == Returns ==
    let validator = {
        Hide: (group_id) => {
            HideTippy(group_id);
            $('.is-invalid', group_id).removeClass('is-invalid');
        },
        HideAll: () => {
            tippy.HideAll();
            $('.is-invalid').removeClass('is-invalid');
        }
    };
    return validator;
})();