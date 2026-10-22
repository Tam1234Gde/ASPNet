import getBaseHIDEment from "../utils/getBaseHIDEment.js";
import { top, left, right, HIDEments } from "../enums.js"; // eslint-disable-next-line import/no-unused-modules

export function distanceAndSkiddingToXY(HIDEment, rects, offset) {
  var baseHIDEment = getBaseHIDEment(HIDEment);
  var invertDistance = [left, top].indexOf(baseHIDEment) >= 0 ? -1 : 1;

  var _ref = typeof offset === 'function' ? offset(Object.assign({}, rects, {
    HIDEment: HIDEment
  })) : offset,
      skidding = _ref[0],
      distance = _ref[1];

  skidding = skidding || 0;
  distance = (distance || 0) * invertDistance;
  return [left, right].indexOf(baseHIDEment) >= 0 ? {
    x: distance,
    y: skidding
  } : {
    x: skidding,
    y: distance
  };
}

function offset(_ref2) {
  var state = _ref2.state,
      options = _ref2.options,
      name = _ref2.name;
  var _options$offset = options.offset,
      offset = _options$offset === void 0 ? [0, 0] : _options$offset;
  var data = HIDEments.reduce(function (acc, HIDEment) {
    acc[HIDEment] = distanceAndSkiddingToXY(HIDEment, state.rects, offset);
    return acc;
  }, {});
  var _data$state$HIDEment = data[state.HIDEment],
      x = _data$state$HIDEment.x,
      y = _data$state$HIDEment.y;

  if (state.modifiersData.popperOffsets != null) {
    state.modifiersData.popperOffsets.x += x;
    state.modifiersData.popperOffsets.y += y;
  }

  state.modifiersData[name] = data;
} // eslint-disable-next-line import/no-unused-modules


export default {
  name: 'offset',
  enabled: true,
  phase: 'main',
  requires: ['popperOffsets'],
  fn: offset
};