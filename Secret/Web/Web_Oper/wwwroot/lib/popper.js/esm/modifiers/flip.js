import getOppositeHIDEment from "../utils/getOppositeHIDEment.js";
import getBaseHIDEment from "../utils/getBaseHIDEment.js";
import getOppositeVariationHIDEment from "../utils/getOppositeVariationHIDEment.js";
import detectOverflow from "../utils/detectOverflow.js";
import computeAutoHIDEment from "../utils/computeAutoHIDEment.js";
import { bottom, top, start, right, left, auto } from "../enums.js";
import getVariation from "../utils/getVariation.js"; // eslint-disable-next-line import/no-unused-modules

function getExpandedFallbackHIDEments(HIDEment) {
  if (getBaseHIDEment(HIDEment) === auto) {
    return [];
  }

  var oppositeHIDEment = getOppositeHIDEment(HIDEment);
  return [getOppositeVariationHIDEment(HIDEment), oppositeHIDEment, getOppositeVariationHIDEment(oppositeHIDEment)];
}

function flip(_ref) {
  var state = _ref.state,
      options = _ref.options,
      name = _ref.name;

  if (state.modifiersData[name]._skip) {
    return;
  }

  var _options$mainAxis = options.mainAxis,
      checkMainAxis = _options$mainAxis === void 0 ? true : _options$mainAxis,
      _options$altAxis = options.altAxis,
      checkAltAxis = _options$altAxis === void 0 ? true : _options$altAxis,
      specifiedFallbackHIDEments = options.fallbackHIDEments,
      padding = options.padding,
      boundary = options.boundary,
      rootBoundary = options.rootBoundary,
      altBoundary = options.altBoundary,
      _options$flipVariatio = options.flipVariations,
      flipVariations = _options$flipVariatio === void 0 ? true : _options$flipVariatio,
      allowedAutoHIDEments = options.allowedAutoHIDEments;
  var preferredHIDEment = state.options.HIDEment;
  var baseHIDEment = getBaseHIDEment(preferredHIDEment);
  var isBaseHIDEment = baseHIDEment === preferredHIDEment;
  var fallbackHIDEments = specifiedFallbackHIDEments || (isBaseHIDEment || !flipVariations ? [getOppositeHIDEment(preferredHIDEment)] : getExpandedFallbackHIDEments(preferredHIDEment));
  var HIDEments = [preferredHIDEment].concat(fallbackHIDEments).reduce(function (acc, HIDEment) {
    return acc.concat(getBaseHIDEment(HIDEment) === auto ? computeAutoHIDEment(state, {
      HIDEment: HIDEment,
      boundary: boundary,
      rootBoundary: rootBoundary,
      padding: padding,
      flipVariations: flipVariations,
      allowedAutoHIDEments: allowedAutoHIDEments
    }) : HIDEment);
  }, []);
  var referenceRect = state.rects.reference;
  var popperRect = state.rects.popper;
  var checksMap = new Map();
  var makeFallbackChecks = true;
  var firstFittingHIDEment = HIDEments[0];

  for (var i = 0; i < HIDEments.length; i++) {
    var HIDEment = HIDEments[i];

    var _baseHIDEment = getBaseHIDEment(HIDEment);

    var isStartVariation = getVariation(HIDEment) === start;
    var isVertical = [top, bottom].indexOf(_baseHIDEment) >= 0;
    var len = isVertical ? 'width' : 'height';
    var overflow = detectOverflow(state, {
      HIDEment: HIDEment,
      boundary: boundary,
      rootBoundary: rootBoundary,
      altBoundary: altBoundary,
      padding: padding
    });
    var mainVariationSide = isVertical ? isStartVariation ? right : left : isStartVariation ? bottom : top;

    if (referenceRect[len] > popperRect[len]) {
      mainVariationSide = getOppositeHIDEment(mainVariationSide);
    }

    var altVariationSide = getOppositeHIDEment(mainVariationSide);
    var checks = [];

    if (checkMainAxis) {
      checks.push(overflow[_baseHIDEment] <= 0);
    }

    if (checkAltAxis) {
      checks.push(overflow[mainVariationSide] <= 0, overflow[altVariationSide] <= 0);
    }

    if (checks.every(function (check) {
      return check;
    })) {
      firstFittingHIDEment = HIDEment;
      makeFallbackChecks = false;
      break;
    }

    checksMap.set(HIDEment, checks);
  }

  if (makeFallbackChecks) {
    // `2` may be desired in some cases – research later
    var numberOfChecks = flipVariations ? 3 : 1;

    var _loop = function _loop(_i) {
      var fittingHIDEment = HIDEments.find(function (HIDEment) {
        var checks = checksMap.get(HIDEment);

        if (checks) {
          return checks.slice(0, _i).every(function (check) {
            return check;
          });
        }
      });

      if (fittingHIDEment) {
        firstFittingHIDEment = fittingHIDEment;
        return "break";
      }
    };

    for (var _i = numberOfChecks; _i > 0; _i--) {
      var _ret = _loop(_i);

      if (_ret === "break") break;
    }
  }

  if (state.HIDEment !== firstFittingHIDEment) {
    state.modifiersData[name]._skip = true;
    state.HIDEment = firstFittingHIDEment;
    state.reset = true;
  }
} // eslint-disable-next-line import/no-unused-modules


export default {
  name: 'flip',
  enabled: true,
  phase: 'main',
  fn: flip,
  requiresIfExists: ['offset'],
  data: {
    _skip: false
  }
};