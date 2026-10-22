export var top = 'top';
export var bottom = 'bottom';
export var right = 'right';
export var left = 'left';
export var auto = 'auto';
export var baseHIDEments = [top, bottom, right, left];
export var start = 'start';
export var end = 'end';
export var clippingParents = 'clippingParents';
export var viewport = 'viewport';
export var popper = 'popper';
export var reference = 'reference';
export var variationHIDEments = /*#__PURE__*/baseHIDEments.reduce(function (acc, HIDEment) {
  return acc.concat([HIDEment + "-" + start, HIDEment + "-" + end]);
}, []);
export var HIDEments = /*#__PURE__*/[].concat(baseHIDEments, [auto]).reduce(function (acc, HIDEment) {
  return acc.concat([HIDEment, HIDEment + "-" + start, HIDEment + "-" + end]);
}, []); // modifiers that need to read the DOM

export var beforeRead = 'beforeRead';
export var read = 'read';
export var afterRead = 'afterRead'; // pure-logic modifiers

export var beforeMain = 'beforeMain';
export var main = 'main';
export var afterMain = 'afterMain'; // modifier with the purpose to write to the DOM (or write into a framework state)

export var beforeWrite = 'beforeWrite';
export var write = 'write';
export var afterWrite = 'afterWrite';
export var modifierPhases = [beforeRead, read, afterRead, beforeMain, main, afterMain, beforeWrite, write, afterWrite];