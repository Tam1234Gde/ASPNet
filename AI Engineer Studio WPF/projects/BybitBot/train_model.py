"""Создаёт демонстрационную ONNX-модель (5 признаков -> вероятности Flat/Long/Short)."""
# pip install numpy onnx sklearn-onnx scikit-learn
import numpy as np
from sklearn.linear_model import LogisticRegression
from sklearn.pipeline import Pipeline
from sklearn.preprocessing import StandardScaler
from skl2onnx import to_onnx

rng = np.random.default_rng(42)
x = rng.normal(size=(3000, 5)).astype(np.float32)
# Игрушечная целевая переменная. Замените историческими размеченными данными.
y = np.where(x[:, 0] > .45, 1, np.where(x[:, 0] < -.45, 2, 0))
model = Pipeline([("scale", StandardScaler()), ("classifier", LogisticRegression(max_iter=1000, multi_class="multinomial"))]).fit(x, y)
onnx = to_onnx(model, x[:1], target_opset=17, options={id(model.steps[-1][1]): {"zipmap": False}})
open("model.onnx", "wb").write(onnx.SerializeToString())
print("Создан model.onnx. Вход: features [1,5], выход: probabilities [1,3]")