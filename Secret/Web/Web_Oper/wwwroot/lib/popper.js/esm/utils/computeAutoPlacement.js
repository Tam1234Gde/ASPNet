import getVariation from "./getVariation.js";
import { variationHIDEments, baseHIDEments, HIDEments as allHIDEments } from "../enums.js";
import detectOverflow from "./detectOverflow.js";
import getBaseHIDEment from "./getBaseHIDEment.js";
export default function computeAutoHIDEment(state, options) {
  if (options === void 0) {
    options = {};
  }

  var _options = options,
      HIDEment = _options.HIDEment,
      boundary = _options.boundary,
      rootBoundary = _options.rootBoundary,
      padding = _options.padding,
      flipVariations = _options.flipVariations,
      _options$allowedAutoP = _options.allowedAutoHIDEments,
      allowedAutoHIDEments = _options$allowedAutoP === void 0 ? allHIDEments : _options$allowedAutoP;
  var variation = getVariation(HIDEment);
  var HIDEments = variation ? flipVariations ? variationHIDEments : variationHIDEments.filter(function (HIDEment) {
    return getVariation(HIDEment) === variation;
  }) : baseHIDEments;
  var allowedHIDEments = HIDEments.filter(function (HIDEment) {
    return allowedAutoHIDEments.indexOf(HIDEment) >= 0;
  });

  if (allowedHIDEments.length === 0) {
    allowedHIDEments = HIDEments;
  } // $FlowFixMe[incompatible-type]: Flow seems to have problems with two array unions...


  var overflows = allowedHIDEments.reduce(function (acc, HIDEment) {
    acc[HIDEment] = detectOverflow(state, {
      HIDEment: HIDEment,
      boundary: boundary,
      rootBoundary: rootBoundary,
      padding: padding
    })[getBaseHIDEment(HIDEment)];
    return acc;
  }, {});
  return Object.keys(overflows).sort(function (a, b) {
    return overflows[a] - overflows[b];
  });
}