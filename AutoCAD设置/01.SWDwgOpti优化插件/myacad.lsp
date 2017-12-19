
;==================================================
;===================自定义快捷键命令===================
;==================================================

;cv=移动
(defun c:cv()
 (command "move")
)

;cc=复制
(defun c:cc()

 (command "copy")
)

;qs=快速选择
(defun c:qs()
 (command "qselect")
)
;ff=填充
(defun c:ff()
 (command "h")
)
;zx=直线
(defun c:zx()
 (command "line")
)
;sx=射线
(defun c:sx()
 (command "xline")
)
;tc=图层
(defun c:tc()
 (command "LAYER")
)
;ff=格式刷
(defun c:fa()
 (command "matchprop")
)
;dd=线性标注
(defun c:dd()
 (command "dimlinear")
)
;ddr=半径标注
(defun c:ddr()
 (command "dimradius")
)
;ddf=直径标注
(defun c:ddf()
 (command "dimdiameter")
)
;dda=角度标注
(defun c:dda()
 (command "dimangular")
)
;ss=特性
(defun c:ss()
 (command "properties")
)


;==================================================
;=====================插件相关命令====================
;==================================================

;在支持路径中查找dvb插件然后加载
(vl-vbaload (findfile"SWDwgOpti.dvb"))

;sw=SolidWorks转化的Dwg优化命令
(defun  c:sw()
(vl-vbarun "SubProgram.AutoRun"))

;gbd=添加国标标注
(defun  c:gbd()
(vl-vbarun "SubProgram.MyDim"))

;hz=添加HZ字体
(defun  c:hz()
(vl-vbarun "SubProgram.AddHZ"))

;ll=加载自定义线型文件
(defun  c:ll()
(vl-vbarun "SubProgram.LoadLin"))



